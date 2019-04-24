package shinningforce.xxxxxxxxxxx;
import java.text.SimpleDateFormat;
import java.util.Date;
//changes		date			version
//new			2019.04.23		V-1.00

/* changes for current project */
//changes		date
//
public class FormatDate {
    private String mFormat;
    private String mDate;

    public FormatDate(Date date, String format){
        mFormat = format;
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(format);
        mDate = simpleDateFormat.format(date);
    }

    public FormatDate(String date, String format){
        mDate = date;
        mFormat = format;
    }

    public FormatDate(){

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
