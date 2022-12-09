#if __has_include(<HyBid/HyBid-Swift.h>)
    #import <HyBid/HyBid-Swift.h>
#else
    #import "HyBid-Swift.h"Ï
#endif

extern "C" {
  void SetHyBidOffsets(int skipOffset){
      [[HyBidSettings sharedInstance] setVideoSkipOffset:[[HyBidSkipOffset alloc] initWithOffset:@(skipOffset) isCustom:true]];
      [[HyBidSettings sharedInstance] setHtmlSkipOffset:[[HyBidSkipOffset alloc] initWithOffset:@(skipOffset) isCustom:true]];
  }
}