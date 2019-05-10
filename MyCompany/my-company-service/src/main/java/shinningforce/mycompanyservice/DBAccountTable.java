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

    Integer getID() {
        return mID;
    }

    void setID(Integer ID) {
        mID = ID;
    }

    String getUserName() {
        return mUserName;
    }

    void setUserName(String userName) {
        mUserName = userName;
    }

    String getPassword() {
        return mPassword;
    }

    void setPassword(String password) {
        mPassword = password;
    }

    Integer getLvl() {
        return mLvl;
    }

    void setLvl(Integer lvl) {
        mLvl = lvl;
    }

}
