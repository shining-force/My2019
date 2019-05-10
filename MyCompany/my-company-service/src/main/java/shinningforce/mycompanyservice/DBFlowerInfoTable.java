package shinningforce.mycompanyservice;

import javax.persistence.*;

@Entity
@Table(name = "t_DBFlowerInfoTable")
public class DBFlowerInfoTable {
    @Id
    @GeneratedValue(strategy= GenerationType.SEQUENCE)
    private Integer mID;

    @Column(name = "flowerInfo")
    private String mFlowerInfo;
    @Column(name = "flowerName")
    private String mFlowerName;
    @Column(name = "flowerDescription")
    private String mFlowerDescription;

    Integer getID() {
        return mID;
    }

    void setID(Integer ID) {
        mID = ID;
    }

    String getFlowerInfo() {
        return mFlowerInfo;
    }

    void setFlowerInfo(String flowerInfo) {
        mFlowerInfo = flowerInfo;
    }

    String getFlowerName() {
        return mFlowerName;
    }

    void setFlowerName(String flowerName) {
        mFlowerName = flowerName;
    }

    String getFlowerDescription() {
        return mFlowerDescription;
    }

    void setFlowerDescription(String flowerDescription) {
        mFlowerDescription = flowerDescription;
    }
}
