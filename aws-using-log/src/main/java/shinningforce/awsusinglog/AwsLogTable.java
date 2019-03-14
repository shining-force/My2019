package shinningforce.awsusinglog;
import javax.persistence.*;

@Entity
public class AwsLogTable {
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE)
    @Column(name = "id")
    private Integer mID;

    @Column(name = "date")
    private String mFormatDate;
    @Column(name = "date_format")
    private String mFormat;
    @Column(name = "title")
    private String mTitle;
    @Column(name = "detail")
    private String mDetail;
    @Column(name = "from_user")
    private String mFromUser;

    public AwsLog_Transmit toTransmitFormat(){
        AwsLog_Transmit awsLog_transmit = new AwsLog_Transmit();
        awsLog_transmit.mDetail = mDetail;
        awsLog_transmit.mFormat = mFormat;
        awsLog_transmit.mFormatDate = mFormatDate;
        awsLog_transmit.mTitle = mTitle;
        awsLog_transmit.mFromUser = mFromUser;

        return awsLog_transmit;
    }


    public Integer getmID() {
        return mID;
    }

    public void setmID(Integer mID) {
        this.mID = mID;
    }

    public String getmFormat() {
        return mFormat;
    }

    public void setmFormatDate(String mFormatDate) {
        this.mFormatDate = mFormatDate;
    }

    public String getmFormatDate() {
        return mFormatDate;
    }

    public void setmFormat(String mFormat) {
        this.mFormat = mFormat;
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

    public String getmFromUser() {
        return mFromUser;
    }

    public void setmFromUser(String mFromUser) {
        this.mFromUser = mFromUser;
    }
}


