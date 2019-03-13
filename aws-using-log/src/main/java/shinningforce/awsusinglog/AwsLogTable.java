package shinningforce.awsusinglog;
import javax.persistence.*;

@Entity
public class AwsLogTable {
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE)
    private Integer mID;

    @Embedded
    private FormatDate mLogDate;
    private String mTitle;
    private String mDetail;

    public Integer getmID() {
        return mID;
    }

    public void setmID(Integer mID) {
        this.mID = mID;
    }

    public FormatDate getmLogDate() {
        return mLogDate;
    }

    public void setmLogDate(FormatDate mLogDate) {
        this.mLogDate = mLogDate;
    }

    public String getmTitle() {
        return mTitle;
    }

    public void setmTitle(String mTitle) {
        this.mTitle = mTitle;
    }

    public String getmDetail() {
        return mDetail;
    }

    public void setmDetail(String mDetail) {
        this.mDetail = mDetail;
    }
}


