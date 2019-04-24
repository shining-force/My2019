package shinningforce.mycompanyservice;

import javax.persistence.*;

@Entity
@Table(name = "t_DBFlowerPicTable")
public class DBFlowerPicTable {
    @Id
    @GeneratedValue(strategy= GenerationType.SEQUENCE)
    private Integer mID;

    @Column(name = "picData")
    private byte[] mPicData;
    @Column(name = "picName")
    private String mPicName;
    @Column(name = "picType")
    private String mPicType;
    @Column(name = "picDescription")
    private String mPicDescription;


    public Integer getID() {
        return mID;
    }

    public void setID(Integer ID) {
        mID = ID;
    }

    public byte[] getPicData() {
        return mPicData;
    }

    public void setPicData(byte[] picData) {
        mPicData = picData;
    }

    public String getPicName() {
        return mPicName;
    }

    public void setPicName(String picName) {
        mPicName = picName;
    }

    public String getPicType() {
        return mPicType;
    }

    public void setPicType(String picType) {
        mPicType = picType;
    }

    public String getPicDescription() {
        return mPicDescription;
    }

    public void setPicDescription(String picDescription) {
        mPicDescription = picDescription;
    }
}
