package shinningforce.awsusinglog;

public class RequestOutCome {
    private Boolean IsSuccess;
    private String mMsg;

    public Boolean getSuccess() {
        return IsSuccess;
    }

    public void setSuccess(Boolean success) {
        IsSuccess = success;
    }

    public String getmMsg() {
        return mMsg;
    }

    public void setmMsg(String mMsg) {
        this.mMsg = mMsg;
    }
}
