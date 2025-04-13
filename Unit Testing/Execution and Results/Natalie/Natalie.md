
Results
I tried using TestRunner but the manager classes were out of scope so instead i ran the unit tests directly on unity console. 

1. ScoreManager Test
Tested Function: AddStroke()
Result: ScoreManager Test Passed – strokes incremented as expected.

2. SoundFXManager Test
Tested Function: PlaySoundEffect()
Result: SoundFXManager Test Ran – no errors; audio method was invoked (check console/audio for verification).

3. SettingsManager Test
Tested Function: SetVolume()
Result: SettingsManager accepted volume input; method ran without exception.

All tests were executed via ManualTestRunner.cs in Play Mode and validated through Unity Console log outputs.

