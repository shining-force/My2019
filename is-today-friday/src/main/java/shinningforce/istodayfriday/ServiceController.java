package shinningforce.istodayfriday;

import org.springframework.web.bind.annotation.*;

import java.text.SimpleDateFormat;
import java.util.Date;

@RestController
public class ServiceController {
    @GetMapping("/today")
    public OneDay getToday() {
        return new OneDay(new Date());
    }

    @PostMapping("/oneDay")
    public OneDay postOneDay(@RequestBody FormatDate formatDate){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(formatDate.getmFormat());
        try{
            Date date = simpleDateFormat.parse(formatDate.getmDate());
            return new OneDay(date);
        }catch (Exception e){
            return new OneDay(new Date());
        }
    }
}