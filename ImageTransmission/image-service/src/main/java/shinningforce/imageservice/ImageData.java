package shinningforce.imageservice;

import org.springframework.stereotype.Repository;

import java.util.concurrent.ConcurrentLinkedQueue;

@Repository
public class ImageData {
    int mNewest;
    public ConcurrentLinkedQueue<ImageTransmissionType> mImageQueue = new ConcurrentLinkedQueue<>();
}
