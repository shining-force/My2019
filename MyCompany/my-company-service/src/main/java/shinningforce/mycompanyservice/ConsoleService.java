package shinningforce.mycompanyservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.Iterator;
import java.util.List;

@Service
public class ConsoleService {
    private final static String gConsoleCode_Login = "00L1";
    private final static String gConsoleCode_Database_Table_NewData = "11N0";
    private final static String gConsoleCode_Database_Table_UpdateData = "11U0";
    private final static String gConsoleCode_Database_Table_DeleteData = "11D0";
    private final static String gConsoleCode_Database_Table_ReadData = "11R1";

    public ConsoleService(){
    }

    public String CodeHandler(String code, String paramL, String paramU){
        String ret = "";
        switch (code){
            case gConsoleCode_Login:
                break;
            case gConsoleCode_Database_Table_NewData:
                break;
            case gConsoleCode_Database_Table_UpdateData:
                break;
            case gConsoleCode_Database_Table_DeleteData:
                break;
            case gConsoleCode_Database_Table_ReadData:
                break;
            default:
                break;
        }
        return ret;
    }
    @Transactional
    private String LoginService(String md5Code, @Autowired AccountRepository accountRepository){

    }
}
