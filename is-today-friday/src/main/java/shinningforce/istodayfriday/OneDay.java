package shinningforce.istodayfriday;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

public class OneDay {
    private static final String ONE_DAY_FORMAT = "yyyy-MM-dd";
    private final Integer mDayOfWeek;
    private final Boolean mIsFriday;
    private final FormatDate mToday;

    public OneDay(Date date) {
        Calendar today = Calendar.getInstance();
        today.setTime(date);
        mDayOfWeek = today.get(Calendar.DAY_OF_WEEK);
        mIsFriday = (mDayOfWeek == Calendar.FRIDAY);
        mToday = new FormatDate(date, ONE_DAY_FORMAT);
    }

    public Integer getmDayOfWeek(){
        return mDayOfWeek;
    }

    public Boolean getmIsFriday(){
        return mIsFriday;
    }

    public FormatDate getmToday(){
        return mToday;
    }
}
