#import "HyprMXUNativeAudio.h"
#import "HyprMXUAudioDelegateImplementation.h"

#ifdef __cplusplus
extern "C" {
#endif
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif

static HyprMXUAudioDelegateImplementation *HyprMXUAudioDelegateInstance = nil;

void HyprMXUConnectAudioListener(bool shouldStart) {
    NSLog(@"HyprMXUConnectAudioListener");
    if (HyprMXUAudioDelegateInstance != nil) {
        return;
    }
    HyprMXUAudioDelegateImplementation *delegate = [HyprMXUAudioDelegateImplementation new];
    delegate.audioStartCompletion = ^{
        UnitySendMessage("HyprMXAudioEventBus", "onAudioStart", "");
    };
    delegate.audioEndCompletion = ^{
        UnitySendMessage("HyprMXAudioEventBus", "onAudioEnd", "");
    };
    HyprMXUAudioDelegateInstance = delegate;
}
