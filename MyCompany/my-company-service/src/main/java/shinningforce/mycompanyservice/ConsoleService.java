package shinningforce.mycompanyservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class ConsoleService {
    private final static String sConsoleCode_Database_Table_NewData = "11N0";
    private final static String sConsoleCode_Database_Table_UpdateData = "11U0";
    private final static String sConsoleCode_Database_Table_DeleteData = "11D0";
    private final static String sConsoleCode_Database_Table_ReadData = "11R1";

    private final static String sRepository_Account = "RPAT";
    private final static String sRepository_FloerInfo = "RPFI";
    private final static String getsRepository_FloerPic = "RPFP";

    private RepositoryAccount mRepositoryAccount;
    private RepositoryFlowerInfo mRepositoryFlowerInfo;
    private RepositoryFlowerPic mRepositoryFlowerPic;

    @Autowired
    public ConsoleService(RepositoryAccount repositoryAccount, RepositoryFlowerInfo repositoryFlowerInfo, RepositoryFlowerPic repositoryFlowerPic){
        mRepositoryAccount = repositoryAccount;
        mRepositoryFlowerInfo = repositoryFlowerInfo;
        mRepositoryFlowerPic = repositoryFlowerPic;
    }

    public String CodeHandler(String code, String paramL, String paramU){
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

    private String handleNewData(String[] params){
        switch (params[0]){
            case sRepository_Account:
                DBAccountTable newAccount = new DBAccountTable();
                newAccount.setUserName(params[1]);
                newAccount.setPassword(params[2]);
                newAccount.setLvl(Integer.parseInt(params[3]));
                mRepositoryAccount.save(newAccount);
                break;
            case sRepository_FloerInfo:
                DBFlowerInfoTable newFlowerInfo = new DBFlowerInfoTable();

                break;
        }
    }
    private String handleUpdateData(String[] params){

    }
    private String handleDeleteData(String[] params){

    }
    private String handleReadData(String[] params){

    }
}
