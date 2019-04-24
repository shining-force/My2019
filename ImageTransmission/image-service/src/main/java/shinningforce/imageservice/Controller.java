package shinningforce.imageservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.ResponseEntity;

import java.util.Timer;
import java.util.TimerTask;

@RestController
public class Controller {
    @Autowired
    private ImageData mImageData;

    @RequestMapping(path = "/upload", method = RequestMethod.POST)
    public ResponseEntity<Integer> upload(@RequestBody ImageTransmissionType image){

        if(mImageData.mLifeCheckTimer == null){
            mImageData.mLifeCheckTimer = new Timer();
            TimerTask timerTask = new TimerTask(){
                @Override
                public void run() {
                    if(!mImageData.mDead ){
                        mImageData.mDead = true;
                    }else{
                        mImageData.mImageQueue.clear();
                        mImageData.mCleaned = true;
                        mImageData.mLifeCheckTimer.cancel();
                        mImageData.mLifeCheckTimer = null;
                    }
                }
            };
            mImageData.mLifeCheckTimer.scheduleAtFixedRate(timerTask, 2000, 2500);
        }
        mImageData.mDead = false;
        mImageData.mCleaned = false;
        mImageData.mImageQueue.add(image);
        if(mImageData.mImageQueue.size() > 5){
            mImageData.mImageQueue.poll();
        }

        return ResponseEntity.ok().body(image.mImageProgress);
    }

    @RequestMapping(path = "/download", method = RequestMethod.GET)
    public ResponseEntity<ImageTransmissionType> download(@RequestParam(name = "imgProgress", defaultValue = "0")int imgProgress){

        ImageTransmissionType transmissionType = null;
        if(mImageData.mCleaned){
            transmissionType = new ImageTransmissionType();
            transmissionType.mImageProgress = 0;
            return ResponseEntity.ok().body(transmissionType);
        }

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
