# UCHomeTask

## 簡介
本專案模擬報價主機與客戶端的系統，實現了透過 UDP 傳輸即時報價資料，並使用 TCP 支援商品訂閱管理功能。

客戶端採用 DataGridView 串接並呈現報價資料，強調即時性與效能。本系統選用 UDP 作為傳輸協定，因其具備「不保證可靠性但高效能」的特性，適合持續推送最新報價資料，滿足使用者專注於最新報價的需求。

---

## 環境需求
- **開發與運行環境**:
  - .NET 8.0
  - 運行於 Windows 作業系統
- **其他需求**:
  - 開啟防火牆規則，允許 UDP 和 TCP 傳輸。
  - 建議使用 Visual Studio 2022 進行開發與調試。

---

## 功能特性

1. **報價主機 (Server)**:
   - 使用 UDP 進行報價資料傳輸。
   - 支援 100 至 1000 檔商品報價模擬。
   - 每檔商品每秒隨機產生 0 ~ 100 筆報價。
   - 使用多任務 (Task) 分離模擬報價、推送報價及網路層的傳輸邏輯。
   - 提供參數化設置，方便調整商品數量。

2. **報價客戶端 (Client)**:
   - 支援多個客戶端同時訂閱不同商品的報價。
   - 使用 DataGridView 即時更新並呈現報價資料。
   - 使用 TCP 管理商品訂閱功能。
   - 支援 Timer 機制避免過度頻繁的畫面刷新。

3. **目前限制**:
   - 尚未實作移除訂閱功能。
   - 尚未實作斷線後的自動重連與訂閱恢復機制。

---

## 如何執行

1. 確認開發環境已安裝 .NET 8.0。
2. 依序啟動以下程式：
   1. 啟動 **Server**，可透過參數指定商品數量（例如：`QuoteServer.exe 500`，預設為 100）。
   2. 啟動 **Client**。
3. 在 **Client** 的輸入框中輸入商品名稱（如 `Stock1` 至 `Stock1000`），以訂閱對應商品的報價。

---

## 測試資料

建議以下方式進行測試：

1. 啟動 **Server**，模擬 500 檔商品，每檔商品每秒隨機生成 0~100 筆報價。
2. 啟動多個 **Client**，分別訂閱以下商品：
   - Client 1: `Stock1` ~ `Stock50`
   - Client 2: `Stock51` ~ `Stock100`
   - Client 3: `Stock101` ~ `Stock150`
3. 確認各 Client 的 DataGridView 能即時更新報價資料。
4. 測試系統效能與穩定性，特別是在多客戶端同時訂閱高頻報價時的表現。

---

## 技術細節

- **通訊協定**:
  - **UDP**: 用於定時推送即時報價資料。由於使用者只在乎最新報價，因此採用 UDP 傳輸協定，持續將最新報價推送至客戶端，而不保證數據的完整性或順序性。
  - **TCP**: 用於管理商品訂閱功能，確保訂閱請求的可靠性。

- **架構設計**:
  - **Server**:
    - 使用多任務 (Task) 分離以下邏輯：
      1. 模擬商品報價。
      2. 將報價推送給訂閱的客戶端。
      3. 處理網路層的報價傳輸。
    - 抽象化網路層，未來可考慮更換更高效的網路層套件（如 ZeroMQ 或 gRPC）。
  - **Client**:
    - 使用 DataGridView 綁定即時更新的報價資料。
    - 採用 Timer 機制定時刷新畫面，避免過度頻繁的操作導致效能下降。

---

## 優化方向

1. **功能強化**:
   - 增加訂閱移除功能，提升客戶端操作靈活性。
   - 實現斷線後的自動重連機制，並確保訂閱恢復。

2. **報價資料改進**:
   - 支援 Ticker 資料傳送，開發技術分析（如技術指標計算）及交易明細追蹤功能。
   - 實現 Ticker 資料回補機制，確保歷史數據完整性。

3. **效能優化**:
   - 深入分析 WinForms 的效能瓶頸，研究虛擬化技術（如 UI 處理大量資料時的內存優化）。
   - 評估多客戶端高頻訂閱時的效能，優化資源管理機制以減少伺服器負載。

4. **驗證與可靠性**:
   - 引入數據完整性檢查機制（如 CRC 校驗），以減少低頻交易時的資料漏傳問題。
   - 考慮採用更高效的傳輸協定（如 QUIC）以進一步提升可靠性。

---

## 專案目標
本專案旨在模擬高頻報價系統，展示即時性、高效能及多用戶支持的架構設計，為金融交易應用提供參考實現。
