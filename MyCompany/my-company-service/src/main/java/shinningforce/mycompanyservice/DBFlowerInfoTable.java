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

    public Integer getID() {
        return mID;
    }

    public void setID(Integer ID) {
        mID = ID;
    }

    public String getFlowerInfo() {
        return mFlowerInfo;
    }

    public void setFlowerInfo(String flowerInfo) {
        mFlowerInfo = flowerInfo;
    }

    public String getFlowerName() {
        return mFlowerName;
    }

    public void setFlowerName(String flowerName) {
        mFlowerName = flowerName;
    }

    public String getFlowerDescription() {
        return mFlowerDescription;
    }

    public void setFlowerDescription(String flowerDescription) {
        mFlowerDescription = flowerDescription;
    }
}
