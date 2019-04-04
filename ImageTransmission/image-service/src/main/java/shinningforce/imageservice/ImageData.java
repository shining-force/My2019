package shinningforce.imageservice;

import org.springframework.stereotype.Repository;

@Repository
public class ImageData {
    public int imageProgress;
    public byte[] imageStream;
    public boolean lock;
}
