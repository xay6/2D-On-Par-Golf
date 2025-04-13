## Unit Test 1: PowerMeterUI Display Logic
- **Feature Tested**: Power meter visual feedback
- **Class/Method**: `PowerMeterUI.ShowPowerMeter()`
- **Goal**: Confirm the `CanvasGroup.alpha` value is set to 1 when showing the meter.
- **Fields Used**: `canvasGroup.alpha`
- **Return Object**: No return, but observable UI change

## Unit Test 2: Random City Selection for API Call
- **Feature Tested**: Weather-based wind system
- **Class/Method**: `Wind.SelectRandomCity()` *(or equivalent method)*
- **Goal**: Ensure one of the valid cities is selected when wind is refreshed
- **Fields Used**: `string[] cities`, `string selectedCity`
- **Return Object**: Selected city string

## Unit Test 3: API Fetch & Wind Strength Parsing
- **Feature Tested**: API response parsing
- **Class/Method**: `Wind.ParseWindData(string json)`
- **Goal**: Verify the wind speed is correctly extracted from a sample JSON and assigned to `windStrength`
- **Fields Used**: `float windStrength`, `Vector2 windDirection`
- **Return Object**: None, but wind values should update