package shinningforce.awsusinglog;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@RestController
public class ServiceController {
    @Autowired
    private LogRepository mLogRepository;
    @Autowired
    private DBAccountRepository mDbAccountRepository;
    @Autowired
    private SWHandler mSWHandler;

    @GetMapping("/UseLogs")
    public ResponseEntity<List<AwsLog_Transmit>> getUserLogs(@RequestParam(value = "mSW", defaultValue = "")String sercetWord,
                                                      @RequestParam(value = "st", defaultValue = "")String startDate,
                                                      @RequestParam(value = "ed", defaultValue = "")String endDate,
                                                      @RequestParam(value = "format", defaultValue = "yyyy-MM-dd")String format) {
        if(mSWHandler.getUserNameFromSW(sercetWord) == null)
            return ResponseEntity.badRequest().body(null);

        Date stDate = new FormatDate(startDate, format).toDateType();
        Date edDate = new FormatDate(endDate, format).toDateType();
        if((stDate == null) || (edDate == null))
            return ResponseEntity.badRequest().body(null);

        List<AwsLog_Transmit> awsLog_transmits = new ArrayList<>();
        for (AwsLogTable log:
             mLogRepository.findAll()) {
            Date logDate = new FormatDate(log.getmFormatDate(), log.getmFormat()).toDateType();
            if(logDate == null)
                continue;
            if((logDate.getTime() <= edDate.getTime() && (logDate.getTime() >= stDate.getTime()))){
                awsLog_transmits.add(log.toTransmitFormat());
            }
        }
        return ResponseEntity.ok().body(awsLog_transmits);
    }


    @GetMapping("/AllUseLogs")
    public ResponseEntity<List<AwsLog_Transmit>> getAllUserLogs(@RequestParam(value = "mSW", defaultValue = "")String sercetWord) {
        if(mSWHandler.getUserNameFromSW(sercetWord) == null)
            return ResponseEntity.badRequest().body(null);

        List<AwsLog_Transmit> log_transmits = new ArrayList<>();
        for (AwsLogTable awsLogTable:
                mLogRepository.findAll()) {
            log_transmits.add(awsLogTable.toTransmitFormat());
        }
        return ResponseEntity.ok().body(log_transmits);
    }
    @PostMapping("/NewLog")
    public ResponseEntity<AwsLogTable> postNewLog(@RequestBody AwsLog_Transmit awsLog_transmit){
        if(mSWHandler.getUserNameFromSW(awsLog_transmit.mSW) == null)
            return ResponseEntity.badRequest().body(null);

        AwsLogTable newTable = new AwsLogTable();
        newTable.setmFormatDate(awsLog_transmit.mFormatDate);
        newTable.setmFormat(awsLog_transmit.mFormat);
        newTable.setmTitle(awsLog_transmit.mTitle);
        newTable.setmDetail(awsLog_transmit.mDetail);
        newTable.setmFromUser(mSWHandler.getUserNameFromSW(awsLog_transmit.mSW));
        mLogRepository.save(newTable);
        return ResponseEntity.ok().body(null);
    }

    @PostMapping("/NewAccount")
    public ResponseEntity<RequestOutCome> postNewAccount(@RequestBody DBAccount_Transmit dbAccount_transmit){
        if(mDbAccountRepository.count() !=0){
            if(mSWHandler.getUserNameFromSW(dbAccount_transmit.mSW) == null)
                return ResponseEntity.badRequest().body(null);
        }
        for (DBAcountTable dbAcountTable:
                mDbAccountRepository.findAll()) {
            if(dbAcountTable.getmUserName().equals(dbAccount_transmit.mUserName)){
                RequestOutCome requestOutCome = new RequestOutCome();
                requestOutCome.setmMsg("Account already exist!");
                requestOutCome.setSuccess(false);
                return ResponseEntity.badRequest().body(requestOutCome);
            }
        }

        DBAcountTable dbAcountTable = new DBAcountTable();
        dbAcountTable.setmUserName(dbAccount_transmit.mUserName);
        dbAcountTable.setmPassword(dbAccount_transmit.mPassword);
        mDbAccountRepository.save(dbAcountTable);
        return ResponseEntity.ok().body(null);
    }

    @PostMapping("/UpdateAccount")
    public ResponseEntity<RequestOutCome> postUpdateAccount(@RequestBody DBAccount_Transmit dbAccount_transmit){
        Integer id = mSWHandler.getIDFromSW(dbAccount_transmit.mSW);
        if(id == null)
            return ResponseEntity.badRequest().body(null);

        mDbAccountRepository.deleteById(id);
        DBAcountTable dbAcountTable = new DBAcountTable();
        dbAcountTable.setmUserName(dbAccount_transmit.mUserName);
        dbAcountTable.setmPassword(dbAccount_transmit.mPassword);

        return ResponseEntity.ok().body(null);
    }
}