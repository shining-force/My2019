// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"
#include <thread>
#include <algorithm>

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
//sort type definations
#define SAD_SORTTYPE_SELECT 0
#define SAD_SORTTYPE_INSERT 1
#define SAD_SORTTYPE_SHELL 2
#define SAD_SORTTYPE_MERGE_TOP 3
#define SAD_SORTTYPE_MERGE_BOTTON 4
#define SAD_SORTTYPE_QUICK 5
#define SAD_SORTTYPE_PQ 6
#define SAD_SORTTYPE_CDEFAULT 7

//sort state defination
#define SAD_SORT_STATE_GETTING_SOURCE 0;
#define SAD_SORT_STATE_SORTING 1;
#define SAD_SORT_STATE_FINISHED 2;

//screen prop
long g_lPicWidth, g_lPicHeight;
HWND g_hDrawArea;
CRect g_rectAreaOfs;
POINT *g_pPixelMap;//init and delete in main
//double g_dPartSetX;
//double g_dPartSetY;
//DC prop
HDC g_hDrawDC;//init and delete in update
HBITMAP g_hCDrawBitMap;//init and delete in update

//pixel grp
COLORREF *g_pPixelGrp;//init and delete in main
long g_lGrpLength;

//flag and info
bool g_bSetFuncEnd;
bool g_bDrawDCOK;
stStartInfo g_stStartInfo;
stStateInfo g_stStateInfo;

//get pixel from screen
void GetScreenPixelGrp()
{
	long lScreenWidth = g_lPicWidth;
	long lScreenHeight = g_lPicHeight;
	//get a copy of screen
	HDC hScreenDC = ::GetDC(NULL);
	HDC hScreenCopy = CreateCompatibleDC(hScreenDC);
	HBITMAP hBitmapCopy = CreateCompatibleBitmap(hScreenDC, lScreenWidth, lScreenHeight);
	::SelectObject(hScreenCopy, hBitmapCopy);
	::BitBlt(hScreenCopy, 0, 0, lScreenWidth, lScreenHeight, hScreenDC, 0, 0, SRCCOPY);
	::ReleaseDC(NULL, hScreenDC);
	long lCounter = 0;
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	//get each pixel
	for (long lWidth = 0; lWidth < lScreenWidth; ++lWidth)
	{
		for (long lHeight = 0; lHeight < lScreenHeight; ++lHeight)
		{
			//if func has to be ended at half way
			if (g_bSetFuncEnd == true)
			{
				DeleteDC(hScreenCopy);
				DeleteObject(hBitmapCopy);
				return;
			}
			//store
			g_pPixelGrp[lCounter++] = ::GetPixel(hScreenCopy, lWidth, lHeight);
			//set progress
			g_stStateInfo.lProgress = lCounter;
		}
	}
	DeleteObject(hBitmapCopy);
	DeleteDC(hScreenCopy);
}

//get pixel from file
void GetFilePixelGrp(CImage *pPic)
{
	long lScreenWidth = g_lPicWidth;
	long lScreenHeight = g_lPicHeight;
	long lCounter = 0;
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	for (long lWidth = 0; lWidth < lScreenWidth; ++lWidth)
	{
		for (long lHeight = 0; lHeight < lScreenHeight; ++lHeight)
		{
			//if func has to be ended at half way
			if (g_bSetFuncEnd == true)
				return;
			//store
			g_pPixelGrp[lCounter++] = pPic->GetPixel(lWidth, lHeight);
			//set progress
			g_stStateInfo.lProgress = lCounter;
		}
	}
}

//pixel remapping
void PixelRemapping()
{
	CRect rectDraw;
	GetClientRect(g_hDrawArea, &rectDraw);
	//work out rate
	double dPartSetX = (double)g_lPicWidth / rectDraw.Width();
	double dPartSetY = (double)g_lPicHeight / rectDraw.Height();
	//g_dPartSetX = (double)g_lPicWidth / rectDraw.Width();
	//g_dPartSetY = (double)g_lPicHeight / rectDraw.Height();
	if (dPartSetX < 1) dPartSetX = 1;
	if (dPartSetY < 1) dPartSetY = 1;
	long lScreenPixelX = 0;
	long lScreenPixelY = 0;
	long lMapping;
	COLORREF lScreenPixel;

	//stop updating, make sure SetPixel success
	g_bDrawDCOK = false;
	//reset each pixel in draw DC and update pixel map
	for (long lWidth = 0; lWidth < rectDraw.Width(); ++lWidth)
	{
		if (lWidth > g_lPicWidth)
		{
			break;
		}
		lScreenPixelX = lWidth * dPartSetX;
		//lScreenPixelX = lWidth * g_dPartSetX;
		for (long lHeight = 0; lHeight < rectDraw.Height(); ++lHeight)
		{
			if (lHeight > g_lPicHeight)
			{
				break;
			}
			lScreenPixelY = lHeight * dPartSetY;
			//lScreenPixelY = lHeight * g_dPartSetY;
			lMapping = lScreenPixelX * g_lPicHeight + lScreenPixelY;
			lScreenPixel = g_pPixelGrp[lMapping];
			::SetPixel(g_hDrawDC, lWidth, lHeight, lScreenPixel);
			g_pPixelMap[lMapping].x = lWidth;
			g_pPixelMap[lMapping].y = lHeight;
		}
	}
	//draw ok
	g_bDrawDCOK = true;
}

//update draw area
DWORD WINAPI  UpdateDrawArea(LPVOID lpParameter)
{
	//create DC
	CRect rectDraw;
	HDC hClientDC;
	hClientDC = ::GetDC(g_hDrawArea);
	GetClientRect(g_hDrawArea, &rectDraw);
	g_hDrawDC = CreateCompatibleDC(hClientDC);
	g_hCDrawBitMap = CreateCompatibleBitmap(hClientDC, rectDraw.Width(), rectDraw.Height());
	::SelectObject(g_hDrawDC, g_hCDrawBitMap);
	::ReleaseDC(g_hDrawArea, hClientDC);
	g_bDrawDCOK = true;

	//update loop
	while (true)
	{
		//CRect rectCurDraw;
		////get current draw area rect
		//GetClientRect(g_hDrawArea, &rectCurDraw);
		//if ((rectDraw.Width() != rectCurDraw.Width()) || (rectDraw.Height() != rectCurDraw.Height()))
		//{
		//	g_bDrawDCOK = false;

		//	//reset bitmap
		//	DeleteObject(g_hCDrawBitMap);
		//	g_hCDrawBitMap = CreateCompatibleBitmap(hClientDC, rectCurDraw.Width(), rectCurDraw.Height());
		//	::SelectObject(g_hDrawDC, g_hCDrawBitMap);

		//	//reset pixel mapping
		//	PixelRemapping();

		//	//store current
		//	rectDraw = rectCurDraw;
		//	g_bDrawDCOK = true;
		//}
		if (g_bDrawDCOK != false)
		{
			HDC hClinetDC = ::GetDC(g_hDrawArea);
			::BitBlt(hClinetDC, 0, 0, rectDraw.Width(), rectDraw.Height(), g_hDrawDC, 0, 0, SRCCOPY);
			::ReleaseDC(g_hDrawArea, hClinetDC);
		}
		//wait 20ms
		Sleep(20);
		if (g_bSetFuncEnd == true)
			break;
	}

	//delete
	DeleteDC(g_hDrawDC);
	DeleteObject(g_hCDrawBitMap);
	return 0;
}

//state output
void GetSortState(__int32 &lProgress, __int32 &lProgressRange, __int32 &lIsFunctionEnd, __int32 &lSortResult, __int32 &lSortState, __int32 &lSortTime)
{
	lProgress = g_stStateInfo.lProgress;
	lProgressRange = g_stStateInfo.lProgressRange;
	lIsFunctionEnd = g_stStateInfo.lIsFuncEnd;
	lSortResult = g_stStateInfo.lSortResult;
	lSortState = g_stStateInfo.lSortState;
	lSortTime = g_stStateInfo.lSortTime;
}

//select sort
void SelectSort()
{
	long lMin = 0;
	COLORREF lSwitchTemp;
	long lDrawWidth;
	long lDrawHeight;
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	for (long lSwitchLoop = 0; lSwitchLoop < g_lGrpLength; ++lSwitchLoop)
	{
		//update progress
		g_stStateInfo.lProgress = lSwitchLoop;
		lMin = lSwitchLoop;
		for (long lMinLoop = lSwitchLoop + 1; lMinLoop < g_lGrpLength; ++lMinLoop)
		{
			if (g_bSetFuncEnd == true) return;
			if (GetSortKey(&g_pPixelGrp[lMinLoop]) < GetSortKey(&g_pPixelGrp[lMin]))
				lMin = lMinLoop;
		}
		lSwitchTemp = g_pPixelGrp[lSwitchLoop];
		g_pPixelGrp[lSwitchLoop] = g_pPixelGrp[lMin];
		g_pPixelGrp[lMin] = lSwitchTemp;
		//set to draw DC
		if (g_bDrawDCOK == true)
		{
			if (g_pPixelMap[lSwitchLoop].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lSwitchLoop].x, g_pPixelMap[lSwitchLoop].y, g_pPixelGrp[lSwitchLoop]);
			if (g_pPixelMap[lMin].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lMin].x, g_pPixelMap[lMin].y, g_pPixelGrp[lMin]);
			//lDrawWidth = lSwitchLoop / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = lSwitchLoop % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lSwitchLoop]);

			//lDrawWidth = lMin / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = lMin % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lMin]);
		}
	}
}
//insert sort
void InsertSort()
{
	COLORREF lSwitchTemp;
	long lDrawWidth;
	long lDrawHeight;
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	for (long lStartLoop = 1; lStartLoop < g_lGrpLength; ++lStartLoop)
	{
		//update progress
		g_stStateInfo.lProgress = lStartLoop;
		for (long lInsertLoop = lStartLoop; lInsertLoop > 0; --lInsertLoop)
		{
			if (g_bSetFuncEnd == true) return;
			if (GetSortKey(&g_pPixelGrp[lInsertLoop]) < GetSortKey(&g_pPixelGrp[lInsertLoop - 1]))
			{
				lSwitchTemp = g_pPixelGrp[lInsertLoop];
				g_pPixelGrp[lInsertLoop] = g_pPixelGrp[lInsertLoop - 1];
				g_pPixelGrp[lInsertLoop - 1] = lSwitchTemp;
				//set to draw
				if (g_bDrawDCOK == true)
				{
					if (g_pPixelMap[lInsertLoop].x > 0)
						::SetPixel(g_hDrawDC, g_pPixelMap[lInsertLoop].x, g_pPixelMap[lInsertLoop].y, g_pPixelGrp[lInsertLoop]);
					if (g_pPixelMap[lInsertLoop - 1].x > 0)
						::SetPixel(g_hDrawDC, g_pPixelMap[lInsertLoop - 1].x, g_pPixelMap[lInsertLoop - 1].y, g_pPixelGrp[lInsertLoop - 1]);
					//lDrawWidth = lInsertLoop / g_lPicHeight / g_dPartSetX;
					//lDrawHeight = lInsertLoop % g_lPicHeight / g_dPartSetY;
					//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lInsertLoop]);

					//lDrawWidth = (lInsertLoop - 1) / g_lPicHeight / g_dPartSetX;
					//lDrawHeight = (lInsertLoop - 1) % g_lPicHeight / g_dPartSetY;
					//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lInsertLoop - 1)]);
				}
			}
			else break;
		}
	}

}
//shell sort 3H
void ShellSort_3H()
{
	COLORREF lSwitchTemp;
	long lH = 1;
	long lProg = 0;
	long lDrawWidth;
	long lDrawHeight;
	while (lH < g_lGrpLength / 3)
	{
		lH = 3 * lH + 1;
		++lProg;
	}
	//set range
	g_stStateInfo.lProgressRange = lProg;
	lProg = 0;
	while (lH >= 1)
	{
		for (long lStartLoop = lH; lStartLoop < g_lGrpLength; ++lStartLoop)
		{
			for (long lInsertLoop = lStartLoop; lInsertLoop >= lH; lInsertLoop -= lH)
			{
				if (g_bSetFuncEnd == true) return;
				if (GetSortKey(&g_pPixelGrp[lInsertLoop]) < GetSortKey(&g_pPixelGrp[lInsertLoop - lH]))
				{
					lSwitchTemp = g_pPixelGrp[lInsertLoop];
					g_pPixelGrp[lInsertLoop] = g_pPixelGrp[lInsertLoop - lH];
					g_pPixelGrp[lInsertLoop - lH] = lSwitchTemp;
					//set to draw
					if (g_bDrawDCOK == true)
					{
						if (g_pPixelMap[lInsertLoop].x > 0)
							::SetPixel(g_hDrawDC, g_pPixelMap[lInsertLoop].x, g_pPixelMap[lInsertLoop].y, g_pPixelGrp[lInsertLoop]);
						if (g_pPixelMap[lInsertLoop - lH].x > 0)
							::SetPixel(g_hDrawDC, g_pPixelMap[lInsertLoop - lH].x, g_pPixelMap[lInsertLoop - lH].y, g_pPixelGrp[lInsertLoop - lH]);
						//lDrawWidth = lInsertLoop / g_lPicHeight / g_dPartSetX;
						//lDrawHeight = lInsertLoop % g_lPicHeight / g_dPartSetY;
						//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lInsertLoop]);

						//lDrawWidth = (lInsertLoop - lH) / g_lPicHeight / g_dPartSetX;
						//lDrawHeight = (lInsertLoop - lH) % g_lPicHeight / g_dPartSetY;
						//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lInsertLoop - lH)]);
					}
				}
				else break;
			}
		}
		lH /= 3;
		//update progress
		g_stStateInfo.lProgress = lProg++;
	}
}
//merge sort
void MergeSort_Top()
{
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	//get a copy space
	COLORREF *pGrpCopy = new COLORREF[g_lGrpLength];
	MergeSortSub(0, g_lGrpLength - 1, pGrpCopy);
	delete pGrpCopy;
}
void MergeSort_Botton()
{
	//set range
	long lRangeV = 1;
	long lRange = 0;
	while (lRangeV <= g_lGrpLength)
	{
		lRangeV *= 2;
		++lRange;
	}
	g_stStateInfo.lProgressRange = lRange;
	//get a copy space
	COLORREF *pGrpCopy = new COLORREF[g_lGrpLength];
	lRange = 0;
	for (long lGrpSize = 1; lGrpSize < g_lGrpLength; lGrpSize *= 2)
	{
		for (long lMergeIndex = 0; lMergeIndex < g_lGrpLength - lGrpSize; lMergeIndex += lGrpSize * 2)
		{
			if ((lMergeIndex + lGrpSize * 2 - 1) < (g_lGrpLength - 1))
			{
				MergeSortMerge(lMergeIndex, lMergeIndex + lGrpSize - 1, lMergeIndex + lGrpSize * 2 - 1, pGrpCopy);
			}
			else
			{
				MergeSortMerge(lMergeIndex, lMergeIndex + lGrpSize - 1, g_lGrpLength - 1, pGrpCopy);
			}
		}
		g_stStateInfo.lProgress = ++lRange;
	}

	delete pGrpCopy;
}
void MergeSortSub(long lSt, long lEd, COLORREF *pGrpCopy)
{
	if (lSt >= lEd)
		return;
	long lMid = lSt + (lEd - lSt) / 2;// (lSt + lEd) / 2;  maybe out flow
	MergeSortSub(lSt, lMid, pGrpCopy);
	MergeSortSub(lMid + 1, lEd, pGrpCopy);
	MergeSortMerge(lSt, lMid, lEd, pGrpCopy);
	//update progress
	g_stStateInfo.lProgress = (lEd + 1);
}
void MergeSortMerge(long lSt, long lMid, long lEd, COLORREF *pGrpCopy)
{
	long i = lSt, j = lMid + 1;
	long lDrawWidth;
	long lDrawHeight;
	//copy
	memcpy(&pGrpCopy[lSt], &g_pPixelGrp[lSt], (lEd - lSt + 1) * sizeof(COLORREF));
	for (long lMergeLoop = lSt; lMergeLoop <= lEd; ++lMergeLoop)
	{
		if (g_bSetFuncEnd == true) return;
		if (i > lMid)
		{
			g_pPixelGrp[lMergeLoop] = pGrpCopy[j++];
		}
		else if (j > lEd)
		{
			g_pPixelGrp[lMergeLoop] = pGrpCopy[i++];
		}
		else if (GetSortKey(&pGrpCopy[i]) < GetSortKey(&pGrpCopy[j]))
		{
			g_pPixelGrp[lMergeLoop] = pGrpCopy[i++];
		}
		else
		{
			g_pPixelGrp[lMergeLoop] = pGrpCopy[j++];
		}
		//set to draw
		if (g_bDrawDCOK == true)
		{
			if (g_pPixelMap[lMergeLoop].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lMergeLoop].x, g_pPixelMap[lMergeLoop].y, g_pPixelGrp[lMergeLoop]);
		
			//lDrawWidth = lMergeLoop / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = lMergeLoop % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lMergeLoop]);
		}		
	}
}
//quick sort
void QuickSort_Base()
{
	//set range
	g_stStateInfo.lProgressRange = g_lGrpLength;
	g_stStateInfo.lProgress = 0;
	QuickSort_Base_Sub(0, g_lGrpLength - 1);
}

void QuickSort_Base_Sub(long lSt, long lEd)
{
	//set progress here
	++g_stStateInfo.lProgress;
	if (lSt >= lEd)
		return;
	long lDivide = QuickSort_Base_Partition(lSt, lEd);
	if (g_bSetFuncEnd == true) return;
	QuickSort_Base_Sub(lSt, lDivide - 1);
	QuickSort_Base_Sub(lDivide + 1, lEd);
}

long QuickSort_Base_Partition(long lSt, long lEd)
{
	//lSt is to be set
	long lTargetValue = GetSortKey(&g_pPixelGrp[lSt]);
	COLORREF lSwitchTemp;
	long lDrawWidth;
	long lDrawHeight;
	//start pos, suit with (++N) format
	long lLeftIndex = lSt;
	long lRightInex = lEd + 1;
	while (true)
	{
		if (g_bSetFuncEnd == true) return 0;
		//left->right
		while (GetSortKey(&g_pPixelGrp[++lLeftIndex]) < lTargetValue)
		{
			if (g_bSetFuncEnd == true) return 0;
			if (lLeftIndex > lEd) break;
		}
		//right <- left
		while (GetSortKey(&g_pPixelGrp[--lRightInex]) > lTargetValue)
		{
			if (g_bSetFuncEnd == true) return 0;
			if (lRightInex <= lSt) break;
		}
		if (lLeftIndex >= lRightInex)
			break;
		//switch
		lSwitchTemp = g_pPixelGrp[lRightInex];
		g_pPixelGrp[lRightInex] = g_pPixelGrp[lLeftIndex];
		g_pPixelGrp[lLeftIndex] = lSwitchTemp;

		//set to draw
		if (g_bDrawDCOK == true)
		{
			if (g_pPixelMap[lRightInex].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lRightInex].x, g_pPixelMap[lRightInex].y, g_pPixelGrp[lRightInex]);
			if (g_pPixelMap[lLeftIndex].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lLeftIndex].x, g_pPixelMap[lLeftIndex].y, g_pPixelGrp[lLeftIndex]);
			//lDrawWidth = lRightInex / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = lRightInex % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lRightInex]);

			//lDrawWidth = (lLeftIndex) / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = (lLeftIndex) % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lLeftIndex)]);
		}
	}
	//switch lSt to right pos
	lSwitchTemp = g_pPixelGrp[lSt];
	g_pPixelGrp[lSt] = g_pPixelGrp[lRightInex];
	g_pPixelGrp[lRightInex] = lSwitchTemp;
	//set to draw
	if (g_bDrawDCOK == true)
	{
		if (g_pPixelMap[lSt].x > 0)
			::SetPixel(g_hDrawDC, g_pPixelMap[lSt].x, g_pPixelMap[lSt].y, g_pPixelGrp[lSt]);
		if (g_pPixelMap[lLeftIndex].x > 0)
			::SetPixel(g_hDrawDC, g_pPixelMap[lLeftIndex].x, g_pPixelMap[lLeftIndex].y, g_pPixelGrp[lLeftIndex]);
		//lDrawWidth = lSt / g_lPicHeight / g_dPartSetX;
		//lDrawHeight = lSt % g_lPicHeight / g_dPartSetY;
		//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lSt]);

		//lDrawWidth = (lLeftIndex) / g_lPicHeight / g_dPartSetX;
		//lDrawHeight = (lLeftIndex) % g_lPicHeight / g_dPartSetY;
		//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lLeftIndex)]);

	}
	return lRightInex;
}
//minQP
void PriorityQueueSort()
{
	COLORREF lSwitchTemp;
	long lDrawWidth;
	long lDrawHeight;
	g_stStateInfo.lProgressRange = (g_lGrpLength - 1) / 2 + g_lGrpLength - 1;
	g_stStateInfo.lProgress = 0;
	//use sink to make a priority queue, big index
	for (long lMakePQ = (g_lGrpLength - 1) / 2; lMakePQ >= 0; --lMakePQ)
	{
		if (g_bSetFuncEnd == true) return ;
		PriorityQueueSort_Sink(lMakePQ, g_lGrpLength - 1);
		++g_stStateInfo.lProgress;
	}
	//fix
	long lFixIndex = g_lGrpLength - 1;
	while (lFixIndex > 0)
	{
		if (g_bSetFuncEnd == true) return;
		//switch the biggest to the last
		lSwitchTemp = g_pPixelGrp[0];
		g_pPixelGrp[0] = g_pPixelGrp[lFixIndex];
		g_pPixelGrp[lFixIndex] = lSwitchTemp;
		//set to draw
		if (g_bDrawDCOK == true)
		{
			if (g_pPixelMap[0].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[0].x, g_pPixelMap[0].y, g_pPixelGrp[0]);
			if (g_pPixelMap[lFixIndex].x > 0)
				::SetPixel(g_hDrawDC, g_pPixelMap[lFixIndex].x, g_pPixelMap[lFixIndex].y, g_pPixelGrp[lFixIndex]);

			//::SetPixel(g_hDrawDC, 0, 0, g_pPixelGrp[0]);

			//lDrawWidth = (lFixIndex) / g_lPicHeight / g_dPartSetX;
			//lDrawHeight = (lFixIndex) % g_lPicHeight / g_dPartSetY;
			//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lFixIndex)]);

		}
		//change sink range
		--lFixIndex;
		//use sink to fix the queue
		PriorityQueueSort_Sink(0, lFixIndex);
		++g_stStateInfo.lProgress;
	}
}
void PriorityQueueSort_Sink(long lSinkIndex, long lEndPos)
{
	COLORREF lSwitchTemp;
	long lDrawWidth;
	long lDrawHeight;
	long lChildIndex;
	while ((2 * lSinkIndex + 1) <= lEndPos)
	{
		if (g_bSetFuncEnd == true) return;
		lChildIndex = 2 * lSinkIndex + 1;
		//compare child left and child right
		if ((lChildIndex + 1) <= lEndPos)
		{
			if (GetSortKey(&g_pPixelGrp[lChildIndex]) < GetSortKey(&g_pPixelGrp[lChildIndex + 1]))
			{
				//get bigger
				++lChildIndex;
			}
		}

		//compare with child
		if (GetSortKey(&g_pPixelGrp[lSinkIndex]) < GetSortKey(&g_pPixelGrp[lChildIndex]))
		{
			//switch
			lSwitchTemp = g_pPixelGrp[lSinkIndex];
			g_pPixelGrp[lSinkIndex] = g_pPixelGrp[lChildIndex];
			g_pPixelGrp[lChildIndex] = lSwitchTemp;
			//set to draw
			if (g_bDrawDCOK == true)
			{
				if (g_pPixelMap[lSinkIndex].x > 0)
					::SetPixel(g_hDrawDC, g_pPixelMap[lSinkIndex].x, g_pPixelMap[lSinkIndex].y, g_pPixelGrp[lSinkIndex]);
				if (g_pPixelMap[lChildIndex].x > 0)
					::SetPixel(g_hDrawDC, g_pPixelMap[lChildIndex].x, g_pPixelMap[lChildIndex].y, g_pPixelGrp[lChildIndex]);
				//lDrawWidth = lSinkIndex / g_lPicHeight / g_dPartSetX;
				//lDrawHeight = lSinkIndex % g_lPicHeight / g_dPartSetY;
				//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[lSinkIndex]);

				//lDrawWidth = (lChildIndex) / g_lPicHeight / g_dPartSetX;
				//lDrawHeight = (lChildIndex) % g_lPicHeight / g_dPartSetY;
				//::SetPixel(g_hDrawDC, lDrawWidth, lDrawHeight, g_pPixelGrp[(lChildIndex)]);

			}
			//continue 
			lSinkIndex = lChildIndex;
		}
		else
		{
			//sink complete
			break;
		}
	}
}

//c's default
int CDefaultCompare(const void *pP1, const void *pP2)
{
	return (*(COLORREF*)pP1 - *(COLORREF*)pP2);
}

//get sort key
long GetSortKey(COLORREF* p)
{
	//as long (for now)
	return *p;
}

//check sort
long CheckSort()
{
	for (long lCheckLoop = 0; lCheckLoop < g_lGrpLength - 1; ++lCheckLoop)
	{
		if (GetSortKey(&g_pPixelGrp[lCheckLoop]) > GetSortKey(&g_pPixelGrp[lCheckLoop + 1]))
		{
			return lCheckLoop + 1;
		}
	}
	return 0;
}

//SortAlgorithmStart
void SortAlgorithmStart(wchar_t* szFile, __int32 lSortType, __int32 lShowLiveProgress, HWND hDrawWnd)
{
	//init some normal var
	g_bSetFuncEnd = false;
	g_bDrawDCOK = false;
	memset(&g_stStateInfo, 0, sizeof(g_stStateInfo));

	//store start info and begin main thread
	g_stStartInfo.pFile = szFile;
	g_stStartInfo.lSortType = lSortType;
	g_stStartInfo.bShowLiveProgress = lShowLiveProgress;
	g_hDrawArea = hDrawWnd;
	std::thread main(TestMain, &g_stStartInfo);
	main.detach();
}

//SortMainThread
DWORD WINAPI TestMain(LPVOID lpParameter)
{
	g_stStateInfo.lIsFuncEnd = FALSE;
	stStartInfo *pInfo = (stStartInfo*)lpParameter;

	//begin update draw thread
	std::thread update(UpdateDrawArea, (LPVOID)NULL);
	update.detach();
	
	//get data
	g_stStateInfo.lSortState = SAD_SORT_STATE_GETTING_SOURCE;
	if (pInfo->pFile == NULL)
	{
		//get screen pixel
		g_lPicWidth = GetSystemMetrics(SM_CXSCREEN);      
		g_lPicHeight = GetSystemMetrics(SM_CYSCREEN); 
		g_lGrpLength = g_lPicWidth * g_lPicHeight;
		g_pPixelGrp = new COLORREF[g_lGrpLength];
		g_pPixelMap = new POINT[g_lGrpLength];
		GetScreenPixelGrp();
	}
	else
	{
		//get file pixel
		CImage cImage;
		cImage.Load(pInfo->pFile);
		g_lPicWidth = cImage.GetWidth();
		g_lPicHeight = cImage.GetHeight();
		g_lGrpLength = g_lPicWidth * g_lPicHeight;
		g_pPixelGrp = new COLORREF[g_lGrpLength];
		g_pPixelMap = new POINT[g_lGrpLength];
		GetFilePixelGrp(&cImage);
	}

	//remap pixel
	PixelRemapping();

	//sort part
	g_stStateInfo.lSortState = SAD_SORT_STATE_SORTING;
	if (g_stStartInfo.bShowLiveProgress == false)
		g_bDrawDCOK = false;
	//timer start
	SYSTEMTIME tmSys;
	GetLocalTime(&tmSys);
	CTime tmStart(tmSys);
	__int64 tmDst0 = __int64(tmStart.GetTime()) * 1000 + tmSys.wMilliseconds;
	switch (pInfo->lSortType)
	{
	case SAD_SORTTYPE_SELECT:
		SelectSort();
		break;
	case SAD_SORTTYPE_INSERT:
		InsertSort();
		break;
	case SAD_SORTTYPE_SHELL:
		ShellSort_3H();
		break;
	case SAD_SORTTYPE_MERGE_TOP:
		MergeSort_Top();
		break;
	case SAD_SORTTYPE_MERGE_BOTTON:
		MergeSort_Botton();
		break;
	case SAD_SORTTYPE_QUICK:
		QuickSort_Base();
		break;
	case SAD_SORTTYPE_PQ:
		PriorityQueueSort();
		break;
	case SAD_SORTTYPE_CDEFAULT:
		std::qsort(g_pPixelGrp, g_lGrpLength, sizeof(COLORREF), &CDefaultCompare);
		break;
	default:
		break;
	}
	GetLocalTime(&tmSys);
	CTime tm1(tmSys);
	__int64 tmDst1 = __int64(tm1.GetTime()) * 1000 + tmSys.wMilliseconds;

	//sort time
	g_stStateInfo.lSortTime = tmDst1 - tmDst0;

	//check sort
	g_stStateInfo.lSortResult = CheckSort();
	if (g_stStartInfo.bShowLiveProgress == false)
	{
		g_bDrawDCOK = true;		
	}
	PixelRemapping();

	//delete 
	delete g_pPixelGrp;
	delete g_pPixelMap;
	g_stStateInfo.lIsFuncEnd = TRUE;
	g_stStateInfo.lSortState = SAD_SORT_STATE_FINISHED;
	return 0;
}
//SortAlgorithmStop
void SortAlgorithmStop()
{
	g_bSetFuncEnd = true;
}

