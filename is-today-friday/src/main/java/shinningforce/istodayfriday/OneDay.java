package shinningforce.istodayfriday;

import java.util.Calendar;
import java.util.Date;

public class OneDay {
    private final Integer mDayOfWeek;
    private final Boolean mIsFriday;
    private final Calendar mToday;

    public OneDay(Date date) {
        mToday = Calendar.getInstance();
        mToday.setTime(date);
        mDayOfWeek = mToday.get(Calendar.DAY_OF_WEEK);
        mIsFriday = (mDayOfWeek == Calendar.FRIDAY);
    }

    public Integer getmDayOfWeek(){
        return mDayOfWeek;
    }

    public Boolean getmIsFriday(){
        return mIsFriday;
    }

    public Calendar getmToday(){
        return mToday;
    }
}
