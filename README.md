# WAV 音效檔播放器

這是一個基於 **C# Windows Forms** 開發的音效播放器進階版。除了基礎播放功能外，本版本透過 `mciSendString` 指令強化了**即時進度追蹤**與**進度跳轉**功能，提供更完整的互動體驗。

## 🚀 功能特點

- **檔案選擇**：透過 `OpenFileDialog` 選取本地 `.wav` 檔案。
- **進階播放控制**：
  - **播放/恢復**：自動偵測播放狀態並控制暫存器。
  - **暫停/停止**：精確停止音訊流並重置播放位置。
- **動態進度條 (New!)**：
  - **自動追蹤**：使用 `Timer` 每秒獲取 `status position` 更新進度條。
  - **手動跳轉**：支援拖曳進度條直接跳轉至特定時間點播放。
- **音量調節**：透過底層 `waveOutSetVolume` 硬體層級控制。

## 🖥️ 使用畫面展示

<img width="531" height="316" alt="image" src="https://github.com/user-attachments/assets/4cec2a2f-47fd-4fd9-aad8-4506d067e607" />


### 🎮 操作導引
1.  **載入檔案**：點擊檔案選取按鈕。
2.  **進度操作**：播放時可隨時拖曳下方的進度條，放開滑鼠後會自動跳轉至該位置。
3.  **音量調整**：點擊加減按鈕即可增減 10% 系統音量。

## 🛠️ 技術細節

本專案深度結合 **Windows Multimedia API (`winmm.dll`)**：

*   **MCI 指令集**：
    *   `status myWav length`: 取得音訊總長度作為進度條最大值。
    *   `status myWav position`: 取得目前播放毫秒數。
    *   `seek myWav to [position]`: 實現進度跳轉。
*   **多執行緒安全處理**：
    *   透過 `isDraggingProgress` 旗標，確保手動拖曳進度時不會被 `Timer` 的自動更新回彈，提升 UI 流暢度。

### 核心代碼：進度更新邏輯
```csharp
private void tmrProgress_Tick(object sender, EventArgs e)
{
    if (!isDraggingProgress) // 僅在非拖曳狀態下更新
    {
        StringBuilder sb = new StringBuilder(128);
        mciSendString("status myWav position", sb, sb.Capacity, IntPtr.Zero);
        if (int.TryParse(sb.ToString(), out int position))
        {
            trbProgress.Value = position;
        }
    }
}
