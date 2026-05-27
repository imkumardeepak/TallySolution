import api from './axiosConfig';

const tallyApi = {
  // Company
  getCompany: () => api.get('/Company'),

  // Account Groups
  getAccountGroups: () => api.get('/AccountGroup'),

  // Ledgers
  getLedgers: () => api.get('/AllLedger'),
  saveLedger: (ledger) => api.post('/AllLedger', ledger),

  // Stock Groups
  getStockGroups: () => api.get('/StockGroup'),
  saveStockGroup: (stockgroup) => api.post(`/StockGroup?stockgroup=${encodeURIComponent(stockgroup)}`),

  // Stock Items
  getStockItems: () => api.get('/StockItem'),
  saveStockItem: (stockItem) => api.post('/StockItem', stockItem),

  // UOM
  getUOMs: () => api.get('/UOM'),
  saveUOM: (uom) => api.post(`/UOM?uom=${encodeURIComponent(uom)}`),

  // Vouchers
  getVouchers: () => api.get('/Vouchers'),
  getVoucherTypes: () => api.get('/Vouchers/GetVoucherType'),
  saveVoucher: (voucher) => api.post('/Vouchers', voucher),
};

export default tallyApi;
