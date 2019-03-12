package shinningforce.istodayfriday;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletResponse;
import java.util.Date;

@RestController
public class ServiceController {
    @GetMapping("/today")
    public ResponseEntity<OneDay> getToday() {
        return ResponseEntity.ok(new OneDay(new Date()));
    }

    @GetMapping("/backgroundPic")
    public ResponseEntity<RequestOutCome> getBackgroundPic(@RequestParam(value = "dayOfWeek", defaultValue = "6")Integer dayOfWeek,
            @RequestParam(value = "width", defaultValue = "60")Integer width,
            @RequestParam(value = "height", defaultValue = "60")Integer height,
            HttpServletResponse response){
            DayPicCreater dayPicCreater = new DayPicCreater(dayOfWeek, width, height);
            Throwable ex = dayPicCreater.ImageOutput(response);
            if(ex != null){
                RequestOutCome requestOutCome = new RequestOutCome();
                requestOutCome.setSuccess(false);
                requestOutCome.setmMsg(ex.getLocalizedMessage());
                return ResponseEntity.badRequest().body(requestOutCome);
            }
            else{
                return ResponseEntity.ok().body(null);
            }
    }

    @PostMapping("/oneDay")
    public ResponseEntity<OneDay> postOneDay(@RequestBody FormatDate formatDate){
        Date date = formatDate.toDateType();
        if(date != null){
            return ResponseEntity.ok(new OneDay(date));
        }
        else{
            return ResponseEntity.badRequest().body(null);
        }
    }
}