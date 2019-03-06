// stdafx.h : ��׼ϵͳ�����ļ��İ����ļ���
// ���Ǿ���ʹ�õ��������ĵ�
// �ض�����Ŀ�İ����ļ�
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // �� Windows ͷ���ų�����ʹ�õ�����
// Windows ͷ�ļ�: 
#include <windows.h>
#include <atltypes.h>
#include <atltime.h>
#include <atlimage.h>

// TODO:  �ڴ˴����ó�����Ҫ������ͷ�ļ�
//struct start info
typedef struct ST_START_INFO
{
	wchar_t *pFile;
	__int32 lSortType;
	bool bShowLiveProgress;
}stStartInfo;
//struct state info
typedef struct ST_STATE_INFO
{
	__int32 lProgress;
	__int32 lProgressRange;
	__int32 lIsFuncEnd;
	__int32 lSortState;
	__int32 lSortResult;
	__int32 lSortTime;
}stStateInfo;


//get pixel from screen
void GetScreenPixelGrp();
//get pixel from file
void GetFilePixelGrp(CImage *pPic);
//pixel remapping
void PixelRemapping();
//update draw area
DWORD WINAPI  UpdateDrawArea(LPVOID lpParameter);//in thread, 20ms loop
//select sort
void SelectSort();
//insert sort
void InsertSort();
//shell sort 3H
void ShellSort_3H();
//merge sort
void MergeSort_Top();
void MergeSort_Botton();
void MergeSortSub(long lSt, long lEd, COLORREF *pGrpCopy);
void MergeSortMerge(long lSt, long lMid, long lEd, COLORREF *pGrpCopy);
//quick sort
void QuickSort_Base();
void QuickSort_Base_Sub(long lSt, long lEd);
long QuickSort_Base_Partition(long lSt, long lEd);
//minQP
void PriorityQueueSort();
void PriorityQueueSort_Sink(long lSinkIndex, long lEndPos);
//c's default sort function
int CDefaultCompare(const void *pP1, const void *pP2);
//get sort key
long GetSortKey(COLORREF* p);
//check sort
long CheckSort();
//SortMainThread
DWORD WINAPI TestMain(LPVOID lpParameter);
//state output			<export>
void GetSortState(__int32 &lProgress, __int32 &lProgressRange, __int32 &lIsFunctionEnd, __int32 &lSortResult, __int32 &lSortState, __int32 &lSortTime);
//SortAlgorithmStart	<export>
void SortAlgorithmStart(wchar_t* szFile, __int32 lSortType, __int32 lShowLiveProgress, HWND hDrawWnd);
//SortAlgorithmStop		<export>
void SortAlgorithmStop();

