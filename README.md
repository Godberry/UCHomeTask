# UCHomeTask

## 簡介
此專案為模擬報價主機與客戶端的系統，實現了透過 UDP 傳輸報價資料，並使用 TCP 回補報價的功能。  
客戶端使用 DataGridView 串接並呈現報價資料。  

---

## 環境需求
- **開發與運行環境**:  
  - .NET 8.0
  - 運行於 Windows 作業系統

---

## 功能特性
1. **報價主機 (Server)**:
   - 使用 UDP 進行報價資料傳輸。
   - 支援 100 至 1000 檔商品報價模擬。
   - 每檔商品每秒隨機產生 0 ~ 100 筆報價。
   - 使用 Task 分離模擬報價、推送報價及網路層的傳輸邏輯。

2. **報價客戶端 (Client)**:
   - 支援多個客戶端同時訂閱不同商品的報價。
   - 使用 DataGridView 呈現報價資料。
   - 支援使用 TCP 進行報價回補。

3. **目前限制**:
   - 尚未實作移除訂閱的功能。
   - 尚未實作部分斷線後的處理。

---

## 如何執行
1. 確認開發環境已安裝 .NET 8.0。
2. 依序啟動以下程式：
   1. 啟動 **Server**。(可用參數指定商品數量，預設為100)
   2. 啟動 **Client**。
3. 在 **Client** 輸入框中，輸入商品名稱（如 `Stock1` 至 `Stock1000`），以訂閱對應商品的報價。

---

## 測試資料
本專案未包含預設測試資料，建議以以下方式進行測試：  
1. 在 Client 中輸入任意 `Stock1` ~ `Stock1000` 的商品名稱進行訂閱。(商品編號視Server啟動時給的數量而定)
2. 視覺化確認 DataGridView 是否即時更新報價資料。
3. 測試多個 Client 同時連接，觀察系統效能表現。

---

## 技術細節
- **通訊協定**:
  - **UDP**: 用於定時推送即時報價資料。
  - **TCP**: 用於進行歷史報價資料的回補。
  
- **架構設計**:
  - **Server**:
    - 使用 Task 分離以下邏輯：
      1. 模擬商品報價。
      2. 將報價推送給訂閱的客戶端。
      3. 處理網路層的報價傳輸。
    - 抽象化網路層，未來可考慮更換更高效的網路層套件
  - **Client**:
    - 使用 DataGridView 綁定即時更新的報價資料，並使用timer定時更新避免過度頻繁的更新畫面。
    - 支援多商品訂閱。
