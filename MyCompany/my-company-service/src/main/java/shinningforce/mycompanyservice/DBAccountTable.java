package shinningforce.mycompanyservice;
import javax.persistence.*;
//changes		date			version
//new			2019.04.23		V-1.00

/* changes for current project */
//changes		date
//
@Entity
@Table(name = "t_DBAccountTable")
public class DBAccountTable {
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE)
    private Integer mID;

    @Column(name = "userName")
    private String mUserName;
    @Column(name = "password")
    private String mPassword;
    @Column(name = "lv")
    private Integer mLvl;

    public Integer getID() {
        return mID;
    }

    public void setID(Integer ID) {
        mID = ID;
    }

    public String getUserName() {
        return mUserName;
    }

    public void setUserName(String userName) {
        mUserName = userName;
    }

    public String getPassword() {
        return mPassword;
    }

    public void setPassword(String password) {
        mPassword = password;
    }

    public Integer getLvl() {
        return mLvl;
    }

    public void setLvl(Integer lvl) {
        mLvl = lvl;
    }

}
