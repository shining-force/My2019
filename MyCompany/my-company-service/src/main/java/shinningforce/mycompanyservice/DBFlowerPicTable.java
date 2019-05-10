package shinningforce.mycompanyservice;

import javassist.bytecode.ByteArray;

import javax.persistence.*;

@Entity
@Table(name = "t_DBFlowerPicTable")
public class DBFlowerPicTable {
    @Id
    @GeneratedValue(strategy= GenerationType.SEQUENCE)
    private Integer mID;

    @Column(name = "picData")
    private Byte[] mPicData;
    @Column(name = "picName")
    private String mPicName;
    @Column(name = "picType")
    private String mPicType;
    @Column(name = "picDescription")
    private String mPicDescription;


    Integer getID() {
        return mID;
    }

    void setID(Integer ID) {
        mID = ID;
    }

    byte[] getPicData() {
        byte[] array = new byte[mPicData.length];
        int index = 0;
        for (Byte B:
             mPicData) {
            array[index++] = B;
        }
        return array;
    }


    void setPicData(byte[] picData) {
        Byte[] array = new Byte[picData.length];
        int index = 0;
        for (byte b:
             picData) {
            array[index++] = b;
        }
        mPicData = array;
    }

    String getPicName() {
        return mPicName;
    }

    void setPicName(String picName) {
        mPicName = picName;
    }

    String getPicType() {
        return mPicType;
    }

    void setPicType(String picType) {
        mPicType = picType;
    }

    String getPicDescription() {
        return mPicDescription;
    }

    void setPicDescription(String picDescription) {
        mPicDescription = picDescription;
    }
}
