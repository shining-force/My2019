package shinningforce.mycompanyservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;
import sun.misc.BASE64Decoder;
import sun.misc.BASE64Encoder;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.util.Optional;

//            code       paramU
//connectTest 00T1       Connection OK
//new         11N1       [table]&...
//                       account&[name]&[password]&[lvl]
//                       info&[name]&[description]&[info]
//                       pic&[name]&[description]&[type]&[data]
//update      11U0       [table]&...
//                       account&[id]&[name]&[password]&[lvl]
//                       info&[id]&[name]&[description]&[info]
//                       pic&[id]&[name]&[description]&[type]&[data]
//delete      11D0       [table]&...
//                       account&[id]
//                       info&[id]
//                       pic&[id]
//read        11R1       [table]&...
//                       account&[id]   ret=[name]&[password]&[lvl]
//                       info&[id]      ret=[name]&[description]&[info]
//                       pic&[id]       ret=[name]&[description]&[type]&[data]
//page        11P1       [table]&[stNo]&[count]      ret=[id]&[id]...&[id]&
//info        10I1       ret=[tableNum]&[table1]&[dataNum]&[table2]&[dataNum]...&[tableN]&[dataNum]

@Service
public class ConsoleService {
    private final static String sConsoleCode_Connection_Test = "00T1";
    private final static String sConsoleCode_Database_Table_NewData = "11N1";
    private final static String sConsoleCode_Database_Table_UpdateData = "11U0";
    private final static String sConsoleCode_Database_Table_DeleteData = "11D0";
    private final static String sConsoleCode_Database_Table_ReadData = "11R1";
    private final static String sConsoleCode_Database_Table_Page = "11P1";
    private final static String sConsoleCode_Database_INFO = "10I1";

    private final static String sRepository_Account = "RPAT";
    private final static String sRepository_FlowerInfo = "RPFI";
    private final static String sRepository_FlowerPic = "RPFP";

    private final static String sFileStoragePath_FlowerPic = "F:\\98_Files\\server\\00_FlowerPic\\";

    private final static String sConsoleService_OK = "OK";
    private final static String sConsoleService_Connection_OK = "Connection OK";
    private final static String sConsoleService_Bad = "Bad";
    private final static int sRepository_PageSize = 100;

    private RepositoryAccount mRepositoryAccount;
    private RepositoryFlowerInfo mRepositoryFlowerInfo;
    private RepositoryFlowerPic mRepositoryFlowerPic;

    @Autowired
    public ConsoleService(RepositoryAccount repositoryAccount, RepositoryFlowerInfo repositoryFlowerInfo, RepositoryFlowerPic repositoryFlowerPic){
        mRepositoryAccount = repositoryAccount;
        mRepositoryFlowerInfo = repositoryFlowerInfo;
        mRepositoryFlowerPic = repositoryFlowerPic;
    }

    ConsoleCodeDownTransmissionType CodeHandler(String code, String paramL, String paramU){
        ConsoleCodeDownTransmissionType anwser = new ConsoleCodeDownTransmissionType();

        if(!checkMd5Code(paramL)){
            return new ConsoleCodeDownTransmissionType();
        }
        String[] params = paramU.split("&");

        switch (code){
            case sConsoleCode_Database_Table_NewData:
                anwser.mServiceAnwser = handleNewData(params);
                anwser.mAnwserType = sConsoleCode_Database_Table_NewData;
                break;
            case sConsoleCode_Database_Table_UpdateData:
                anwser.mServiceAnwser = handleUpdateData(params);
                anwser.mAnwserType = sConsoleCode_Database_Table_UpdateData;
                break;
            case sConsoleCode_Database_Table_DeleteData:
                anwser.mServiceAnwser = handleDeleteData(params);
                anwser.mAnwserType = sConsoleCode_Database_Table_DeleteData;
                break;
            case sConsoleCode_Database_Table_ReadData:
                anwser.mServiceAnwser = handleReadData(params);
                anwser.mAnwserType = sConsoleCode_Database_Table_ReadData;
                break;
            case sConsoleCode_Database_Table_Page:
                anwser.mServiceAnwser = handlePage(params);
                anwser.mAnwserType = sConsoleCode_Database_Table_Page;
                break;
            case sConsoleCode_Database_INFO:
                anwser.mServiceAnwser = handleInfo();
                anwser.mAnwserType = sConsoleCode_Database_INFO;
                break;
            case sConsoleCode_Connection_Test:
                anwser.mServiceAnwser = sConsoleService_Connection_OK;
                anwser.mAnwserType = sConsoleCode_Connection_Test;
                break;
            default:
                break;
        }

        return anwser;
    }

    private boolean checkMd5Code(String md5Code){
        for (DBAccountTable account:
                mRepositoryAccount.findAll()) {
            String accountMd5 = StdMD5Maker.getMD5(account.getUserName() + account.getPassword());
            if(accountMd5 != null){
                if(accountMd5.equals(md5Code)){
                    return true;
                }
            }
        }
        return "testcodeofzmxncbv".equals(md5Code);//todo only in test!!!
    }

    @Transactional
    String handleNewData(String[] params){
        String ret = sConsoleService_Bad;
        switch (params[0]){
            case sRepository_Account:
                if(params.length < 4)
                    break;
                DBAccountTable newAccount = new DBAccountTable();
                newAccount.setUserName(params[1]);
                newAccount.setPassword(params[2]);
                newAccount.setLvl(Integer.parseInt(params[3]));
                mRepositoryAccount.save(newAccount);
                ret = newAccount.getID().toString();
                break;
            case sRepository_FlowerInfo:
                if(params.length < 4)
                    break;
                DBFlowerInfoTable newFlowerInfo = new DBFlowerInfoTable();
                newFlowerInfo.setFlowerName(params[1]);
                newFlowerInfo.setFlowerDescription(params[2]);
                newFlowerInfo.setFlowerInfo(params[3]);
                mRepositoryFlowerInfo.save(newFlowerInfo);
                ret = newFlowerInfo.getID().toString();
                break;
            case sRepository_FlowerPic:
                if(params.length < 5)
                    break;
                DBFlowerPicTable newFlowerPic = new DBFlowerPicTable();
                newFlowerPic.setPicName(params[1]);
                newFlowerPic.setPicDescription(params[2]);
                newFlowerPic.setPicType(params[3]);
                mRepositoryFlowerPic.save(newFlowerPic);//save to get id

                File file = new File(sFileStoragePath_FlowerPic, newFlowerPic.getID().toString());
                try{
                    if(file.exists()){
                        file.delete();
                    }
                    file.createNewFile();
                    FileOutputStream stream = new FileOutputStream(file);
                    BASE64Decoder decoder = new BASE64Decoder();

                    StringBuilder picData = new StringBuilder();
                    picData.append(params[4]);

                    if(params.length > 5){
                        //recover data
                        for(int index = 5; index < params.length; ++index){
                            picData.append("&").append(params[index]);
                        }
                    }

                    stream.write(decoder.decodeBuffer(picData.toString()));
                    stream.close();
                }catch (Exception e){
                    mRepositoryFlowerPic.delete(newFlowerPic);
                    break;
                }

                ret = newFlowerPic.getID().toString();
                break;
            default:
                break;
        }
        return ret;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    String handleUpdateData(String[] params){
        String ret = sConsoleService_Bad;
        switch (params[0]){
            case sRepository_Account:
                if(params.length < 5)
                    break;
                Optional<DBAccountTable> optionalAccount = mRepositoryAccount.findById(Integer.parseInt(params[1]));
                if(!optionalAccount.isPresent()){
                    break;
                }
                DBAccountTable account = optionalAccount.get();
                account.setUserName(params[2]);
                account.setPassword(params[3]);
                account.setLvl(Integer.parseInt(params[4]));
                mRepositoryAccount.save(account);
                ret = sConsoleService_OK;
                break;
            case sRepository_FlowerInfo:
                if(params.length < 5)
                    break;
                Optional<DBFlowerInfoTable> optionalInfo = mRepositoryFlowerInfo.findById(Integer.parseInt(params[1]));
                if(!optionalInfo.isPresent()){
                    break;
                }
                DBFlowerInfoTable flowerInfo = optionalInfo.get();
                flowerInfo.setFlowerName(params[2]);
                flowerInfo.setFlowerDescription(params[3]);
                flowerInfo.setFlowerInfo(params[4]);
                mRepositoryFlowerInfo.save(flowerInfo);
                ret = sConsoleService_OK;
                break;
            case sRepository_FlowerPic:
                if(params.length < 6)
                    break;
                Optional<DBFlowerPicTable> optionalPic = mRepositoryFlowerPic.findById(Integer.parseInt(params[1]));
                if(!optionalPic.isPresent()){
                    break;
                }
                DBFlowerPicTable flowerPic = optionalPic.get();
                flowerPic.setPicName(params[2]);
                flowerPic.setPicDescription(params[3]);
                flowerPic.setPicType(params[4]);

                File file = new File(sFileStoragePath_FlowerPic, flowerPic.getID().toString());
                try{
                    if(file.exists()){
                        file.delete();
                    }
                    file.createNewFile();
                    FileOutputStream stream = new FileOutputStream(file);
                    BASE64Decoder decoder = new BASE64Decoder();
                    StringBuilder picData = new StringBuilder();
                    picData.append(params[5]);

                    if(params.length > 6){
                        //recover data
                        for(int index = 6; index < params.length; ++index){
                            picData.append("&").append(params[index]);
                        }
                    }

                    stream.write(decoder.decodeBuffer(picData.toString()));
                    stream.close();
                }catch (Exception e){
                    break;
                }
                mRepositoryFlowerPic.save(flowerPic);
                ret = sConsoleService_OK;
                break;
            default:
                break;
        }
        return ret;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    String handleDeleteData(String[] params){
        String ret = sConsoleService_Bad;
        switch (params[0]){
            case sRepository_Account:
                if(params.length < 2)
                    break;
                Optional<DBAccountTable> optionalAccount = mRepositoryAccount.findById(Integer.parseInt(params[1]));
                if(!optionalAccount.isPresent()){
                    break;
                }
                mRepositoryAccount.deleteById(Integer.parseInt(params[1]));
                ret = sConsoleService_OK;
                break;
            case sRepository_FlowerInfo:
                if(params.length < 2)
                    break;
                Optional<DBFlowerInfoTable> optionalInfo = mRepositoryFlowerInfo.findById(Integer.parseInt(params[1]));
                if(!optionalInfo.isPresent()){
                    break;
                }
                mRepositoryFlowerInfo.deleteById(Integer.parseInt(params[1]));
                ret = sConsoleService_OK;
                break;
            case sRepository_FlowerPic:
                if(params.length < 2)
                    break;
                Optional<DBFlowerPicTable> optionalPic = mRepositoryFlowerPic.findById(Integer.parseInt(params[1]));
                if(!optionalPic.isPresent()){
                    break;
                }
                //delete this in database first
                mRepositoryFlowerPic.deleteById(Integer.parseInt(params[1]));

                File file = new File(sFileStoragePath_FlowerPic, optionalPic.get().getID().toString());
                try{
                    if(file.exists()){
                        file.delete();
                    }
                }catch (Exception e){
                    //do nothing
                }
                ret = sConsoleService_OK;
                break;
            default:
                break;
        }
        return ret;
    }

    @Transactional
    String handleReadData(String[] params){
        String ret = "";
        switch (params[0]){
            case sRepository_Account:
                if(params.length < 2)
                    break;
                Optional<DBAccountTable> optionalAccount = mRepositoryAccount.findById(Integer.parseInt(params[1]));
                if(!optionalAccount.isPresent()){
                    break;
                }
                DBAccountTable account = optionalAccount.get();
                ret = account.getUserName() + "&" + account.getPassword() + "&" + account.getLvl();
                break;
            case sRepository_FlowerInfo:
                if(params.length < 2)
                    break;
                Optional<DBFlowerInfoTable> optionalInfo = mRepositoryFlowerInfo.findById(Integer.parseInt(params[1]));
                if(!optionalInfo.isPresent()){
                    break;
                }
                DBFlowerInfoTable flowerInfo = optionalInfo.get();

                ret = flowerInfo.getFlowerName() + "&" + flowerInfo.getFlowerDescription() + "&" + flowerInfo.getFlowerInfo();
                break;
            case sRepository_FlowerPic:
                if(params.length < 2)
                    break;
                Optional<DBFlowerPicTable> optionalPic = mRepositoryFlowerPic.findById(Integer.parseInt(params[1]));
                if(!optionalPic.isPresent()){
                    break;
                }
                DBFlowerPicTable flowerPic = optionalPic.get();
                BASE64Encoder encoder = new BASE64Encoder();
                File file = new File(sFileStoragePath_FlowerPic, flowerPic.getID().toString());
                byte[] buf = new byte[(int)file.length()];
                try{
                    FileInputStream stream = new FileInputStream(file);
                    stream.read(buf);
                    stream.close();
                }catch (Exception e){
                    break;
                }
                ret = flowerPic.getPicName() + "&" + flowerPic.getPicDescription() + "&" + flowerPic.getPicType() + "&" + encoder.encode(buf);
                break;
            default:
                break;
        }

        return ret;
    }

    @Transactional
    String handlePage(String[] params){
        String ret;
        StringBuilder builder = new StringBuilder();
        int stNo = Integer.parseInt(params[1]);
        int count = Integer.parseInt(params[2]);
        int pageNo = stNo / sRepository_PageSize;
        int offset = stNo % sRepository_PageSize;

        switch (params[0]){
            case sRepository_Account:
                while(count > 0){
                    Page<DBAccountTable> page = mRepositoryAccount.findAll(PageRequest.of(pageNo, sRepository_PageSize));
                    if(page.getContent().size() <= 0)
                        break;
                    for (DBAccountTable account:
                         page) {
                        if(offset > 0)
                            --offset;
                        else{
                            builder.append(account.getID()).append('&');
                            --count;
                            if(count <= 0)
                                break;
                        }
                    }
                    ++pageNo;
                }
                break;
            case sRepository_FlowerInfo:
                while(count > 0){
                    Page<DBFlowerInfoTable> page = mRepositoryFlowerInfo.findAll(PageRequest.of(pageNo, sRepository_PageSize));
                    if(page.getContent().size() <= 0)
                        break;
                    for (DBFlowerInfoTable info:
                            page) {
                        if(offset > 0)
                            --offset;
                        else{
                            builder.append(info.getID()).append('&');
                            --count;
                            if(count <= 0)
                                break;
                        }
                    }
                    ++pageNo;
                }
                break;
            case sRepository_FlowerPic:
                while(count > 0){
                    Page<DBFlowerPicTable> page = mRepositoryFlowerPic.findAll(PageRequest.of(pageNo, sRepository_PageSize));
                    if(page.getContent().size() <= 0)
                        break;
                    for (DBFlowerPicTable pic:
                            page) {
                        if(offset > 0)
                            --offset;
                        else{
                            builder.append(pic.getID()).append('&');
                            --count;
                            if(count <= 0)
                                break;
                        }
                    }
                    ++pageNo;
                }
                break;
            default:
                break;
        }
        ret = builder.toString();
        if(ret.length() > 0)
            ret = ret.substring(0, ret.length() - 1);
        return ret;
    }

    @Transactional
    String handleInfo() {
        return "3&" +
                sRepository_Account + "&" + mRepositoryAccount.count() + "&" +
                sRepository_FlowerInfo + "&" + mRepositoryFlowerInfo.count() + "&" +
                sRepository_FlowerPic + "&" + mRepositoryFlowerPic.count();
    }
}
