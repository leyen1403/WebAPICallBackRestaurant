
# ZaloPay Callback API (.NET Core + Entity Framework)

## 🧾 Mục đích

Dự án này cung cấp một Web API xây dựng bằng ASP.NET Core để:

- Nhận và xử lý callback từ ZaloPay sau khi khách hàng thanh toán thành công.
- Kiểm tra trạng thái đơn hàng qua API của ZaloPay.
- Lưu thông tin giao dịch vào database (`WAIT_PAYMENT`, `ZaloPayCallbackDetails`...).

---

## 🧩 Công nghệ sử dụng

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (qua DbContext `RestaurantGLBContext`)
- Newtonsoft.Json
- ZaloPay SDK (helper để HMAC xác thực)

---

## 📦 Cấu trúc chính

- `Program.cs`: Cấu hình ứng dụng, khởi tạo Swagger và DI.
- `Models/RestaurantGLBContext.cs`: Khai báo các DbSet như `WaitPayments`, `ZaloPayCallbackDetails`, `MerchantAccountsZalopay`.
- `Controllers/ZaloPayCallbackDetailController.cs`: Controller chính xử lý các API:
  - `/api/ZaloPayCallbackDetail/Callback`: Nhận callback từ ZaloPay
  - `/api/ZaloPayCallbackDetail/CheckOrderStatus`: Kiểm tra trạng thái đơn hàng
  - `/api/ZaloPayCallbackDetail/GetAll`: Truy vấn danh sách giao dịch

---

## 🔁 Luồng hoạt động

1. **ZaloPay thanh toán thành công** → gửi HTTP POST callback tới `/api/ZaloPayCallbackDetail/Callback`.
2. API xác thực `MAC` với `KEY2`, kiểm tra `app_trans_id` có tồn tại trong DB hay không.
3. Nếu có, cập nhật `WAIT_PAYMENT.Status = 5` (thành công), log callback vào bảng `ZaloPayCallbackDetails`.
4. Có thể kiểm tra thủ công bằng gọi API `/CheckOrderStatus`.

---

## 🧪 Test Callback bằng ngrok

Để test callback từ ZaloPay gửi về localhost:

```bash
ngrok http 5000
```

Sau đó trong `callback_url` cấu hình gửi đến:
```
https://{your-ngrok-id}.ngrok-free.app/api/ZaloPayCallbackDetail/Callback
```

---

## ⚙️ Cấu hình connection string

Trong `appsettings.json`, thêm chuỗi kết nối SQL Server:

```json
"ConnectionStrings": {
  "RestaurantGLBConnection": "Data Source=.;Initial Catalog=RestaurantGLB;Integrated Security=True;Encrypt=False"
}
```

---

## 📌 Lưu ý

- `app_trans_id` phải theo định dạng `yymmdd_xxxxx` để ZaloPay xác định giao dịch.
- `KEY1`, `KEY2`, `APP_ID` cần cấu hình đúng từ ZaloPay Merchant Dashboard.
- API có thể mở rộng thêm các trạng thái, log, và tích hợp kiểm soát nhiều merchant.

---

## 📃 License

MIT hoặc tùy bạn quyết định.
