package shinningforce.istodayfriday;


import org.springframework.lang.Nullable;

import java.text.SimpleDateFormat;
import java.util.Date;

public class FormatDate {
    private String mFormat;
    private String mDate;

    public FormatDate(Date date, String format){
        mFormat = format;
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(format);
        mDate = simpleDateFormat.format(date);
    }

    public String getmFormat() {
        return mFormat;
    }

    public String getmDate() {
        return mDate;
    }

    public void setmDate(String mDate) {
        this.mDate = mDate;
    }

    public void setmFormat(String mFormat) {
        this.mFormat = mFormat;
    }

    @Nullable
    public Date toDateType(){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(mFormat);
        Date date;
        try{
            date = simpleDateFormat.parse(mDate);
        }catch (Exception e){
            date = null;
        }
        return date;
    }
}
