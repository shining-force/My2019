package shinningforce.awsusinglog;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletResponse;
import java.util.Date;
import java.util.List;

@RestController
public class ServiceController {
    @Autowired
    private LogRepository logRepository;

    @GetMapping("/UseLogs")
    public ResponseEntity<List<AwsLogTable>> getToday(@RequestParam(value = "SW", defaultValue = "")String sercetWord,
                                                      @RequestParam(value = "st", defaultValue = "")FormatDate startDate,
                                                      @RequestParam(value = "ed", defaultValue = "")FormatDate endDate) {
        return ResponseEntity.ok().body(null);
    }
    @GetMapping("/AllUseLogs")
    public ResponseEntity<Iterable<AwsLogTable>> getToday(@RequestParam(value = "SW", defaultValue = "")String sercetWord) {
        return ResponseEntity.ok().body(logRepository.findAll());
    }
    @PostMapping("/NewLog")
    public ResponseEntity<AwsLogTable> postOneDay(@RequestBody AwsLogTable awsLogTable){
        AwsLogTable newTable = new AwsLogTable();
        newTable.setmLogDate(awsLogTable.getmLogDate());
        newTable.setmTitle(awsLogTable.getmTitle());
        newTable.setmDetail(awsLogTable.getmDetail());
        logRepository.save(newTable);
        return ResponseEntity.ok().body(newTable);
    }
}