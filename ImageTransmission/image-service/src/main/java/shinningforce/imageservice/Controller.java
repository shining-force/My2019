package shinningforce.imageservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.ResponseEntity;

@RestController
public class Controller {
    @Autowired
    private ImageData mImageData;

    @RequestMapping(path = "/upload", method = RequestMethod.POST)
    public ResponseEntity<Integer> upload(@RequestBody ImageTransmissionType image){
        for (ImageTransmissionType img:
                mImageData.mImageQueue){
            if(image.mImageProgress < img.mImageProgress){
                mImageData.mImageQueue.clear();
                break;
            }
        }
        mImageData.mImageQueue.add(image);
        if(mImageData.mImageQueue.size() > 5){
            mImageData.mImageQueue.poll();
        }

        return ResponseEntity.ok().body(image.mImageProgress);
    }

    @RequestMapping(path = "/download", method = RequestMethod.GET)
    public ResponseEntity<ImageTransmissionType> download(@RequestParam(name = "imgProgress", defaultValue = "0")int imgProgress){
        ImageTransmissionType transmissionType = null;
        for (ImageTransmissionType img:
             mImageData.mImageQueue) {
            if(img.mImageProgress > imgProgress){
                if(transmissionType != null){
                    if(img.mImageProgress < transmissionType.mImageProgress){
                        transmissionType = img;
                    }
                }else{
                    transmissionType = img;
                }
            }
        }

        return ResponseEntity.ok().body(transmissionType);
    }
}
