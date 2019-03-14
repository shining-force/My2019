package shinningforce.awsusinglog;

import org.hibernate.annotations.Columns;

import javax.persistence.*;

@Entity
public class DBAcountTable {
    @Id
    @GeneratedValue(strategy=GenerationType.AUTO)
    private Integer mID;

    @Column(name = "userName")
    private String mUserName;
    @Column(name = "Password")
    private String mPassword;

    public Integer getmID() {
        return mID;
    }

    public void setmID(Integer mID) {
        this.mID = mID;
    }

    public String getmUserName() {
        return mUserName;
    }

    public void setmUserName(String mUserName) {
        this.mUserName = mUserName;
    }

    public String getmPassword() {
        return mPassword;
    }

    public void setmPassword(String mPassword) {
        this.mPassword = mPassword;
    }
}
