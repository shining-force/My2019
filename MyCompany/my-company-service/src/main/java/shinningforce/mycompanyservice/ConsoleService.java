package shinningforce.mycompanyservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

@Service
public class ConsoleService {
    private final static String sConsoleCode_Database_Table_NewData = "11N0";
    private final static String sConsoleCode_Database_Table_UpdateData = "11U0";
    private final static String sConsoleCode_Database_Table_DeleteData = "11D0";
    private final static String sConsoleCode_Database_Table_ReadData = "11R1";

    private final static String sRepository_Account = "RPAT";
    private final static String sRepository_FlowerInfo = "RPFI";
    private final static String sRepository_FlowerPic = "RPFP";

    private final static String sConsoleService_OK = "OK";
    private final static String sConsoleService_Bad = "Bad";

    private RepositoryAccount mRepositoryAccount;
    private RepositoryFlowerInfo mRepositoryFlowerInfo;
    private RepositoryFlowerPic mRepositoryFlowerPic;

    @Autowired
    public ConsoleService(RepositoryAccount repositoryAccount, RepositoryFlowerInfo repositoryFlowerInfo, RepositoryFlowerPic repositoryFlowerPic){
        mRepositoryAccount = repositoryAccount;
        mRepositoryFlowerInfo = repositoryFlowerInfo;
        mRepositoryFlowerPic = repositoryFlowerPic;
    }

    String CodeHandler(String code, String paramL, String paramU){
        String ret = "";
        if(!checkMd5Code(paramL)){
            return ret;
        }
        String[] params = paramU.split("&");

        switch (code){
            case sConsoleCode_Database_Table_NewData:
                ret = handleNewData(params);
                break;
            case sConsoleCode_Database_Table_UpdateData:
                ret = handleUpdateData(params);
                break;
            case sConsoleCode_Database_Table_DeleteData:
                ret = handleDeleteData(params);
                break;
            case sConsoleCode_Database_Table_ReadData:
                ret = handleReadData(params);
                break;
            default:
                break;
        }
        return ret;
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
        return false;
    }

    @Transactional
    String handleNewData(String[] params){
        String ref = sConsoleService_Bad;
        switch (params[0]){
            case sRepository_Account:
                if(params.length < 4)
                    break;
                DBAccountTable newAccount = new DBAccountTable();
                newAccount.setUserName(params[1]);
                newAccount.setPassword(params[2]);
                newAccount.setLvl(Integer.parseInt(params[3]));
                mRepositoryAccount.save(newAccount);
                ref = sConsoleService_OK;
                break;
            case sRepository_FlowerInfo:
                if(params.length < 4)
                    break;
                DBFlowerInfoTable newFlowerInfo = new DBFlowerInfoTable();
                newFlowerInfo.setFlowerName(params[1]);
                newFlowerInfo.setFlowerDescription(params[2]);
                newFlowerInfo.setFlowerInfo(params[3]);
                mRepositoryFlowerInfo.save(newFlowerInfo);
                ref = sConsoleService_OK;
                break;
            case sRepository_FlowerPic:
                if(params.length < 5)
                    break;
                DBFlowerPicTable newFlowerPic = new DBFlowerPicTable();
                newFlowerPic.setPicName(params[1]);
                newFlowerPic.setPicDescription(params[2]);
                newFlowerPic.setPicType(params[3]);
                newFlowerPic.setPicData(params[4].getBytes());
                mRepositoryFlowerPic.save(newFlowerPic);
                ref = sConsoleService_OK;
                break;
            default:
                break;
        }
        return ref;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    String handleUpdateData(String[] params){

    }
    private String handleDeleteData(String[] params){

    }
    private String handleReadData(String[] params){

    }
}
