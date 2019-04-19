package shinningforce.imageservice;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;

public class ImageTransmissionType {
    @JsonProperty("m_szImageProgress")
    public Integer mImageProgress;
    @JsonProperty("m_pImgStreamGrp")
    public List<byte[]> mImgStreamGrp;
}
