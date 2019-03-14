package shinningforce.awsusinglog;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.lang.Nullable;
import org.springframework.stereotype.Service;

@Service
public class SWHandler {
    @Autowired
    private DBAccountRepository dbAccountRepository;

    @Nullable
    public String getUserNameFromSW(String sw){
        for (DBAcountTable dbAccountTable:
                dbAccountRepository.findAll()) {
            String swString = dbAccountTable.getmUserName() + dbAccountTable.getmPassword();
            if(sw.equals(StdMD5Maker.getMD5(swString)))
            {
                return dbAccountTable.getmUserName();
            }
        }
        return null;
    }

    @Nullable
    public Integer getIDFromSW(String sw){
        for (DBAcountTable dbAccountTable:
                dbAccountRepository.findAll()) {
            String swString = dbAccountTable.getmUserName() + dbAccountTable.getmPassword();
            if(sw.equals(StdMD5Maker.getMD5(swString)))
            {
                return dbAccountTable.getmID();
            }
        }
        return null;
    }
}
