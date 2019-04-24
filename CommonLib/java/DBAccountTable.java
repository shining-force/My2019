package shinningforce.xxxxxxxxxxx;

import javax.persistence.*;
//changes		date			version
//new			2019.04.23		V-1.00

/* changes for current project */
//changes		date
//
@Entity
public class DBAccountTable {
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE)
    private Integer mID;

    @Column(name = "userName")
    private String mUserName;
    @Column(name = "Password")
    private String mPassword;
    @Column(name = "lv")
    private Integer mLvl;

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

    public void setmLvl(Integer mLvl) {
        this.mLvl = mLvl;
    }

    public Integer getmLvl() {
        return mLvl;
    }
}
