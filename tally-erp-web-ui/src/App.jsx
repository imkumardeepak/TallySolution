import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import theme from './theme';
import MainLayout from './layouts/MainLayout';
import Dashboard from './pages/Dashboard';
import Companies from './pages/Companies';
import AccountGroups from './pages/AccountGroups';
import Ledgers from './pages/Ledgers';
import StockGroups from './pages/StockGroups';
import StockItems from './pages/StockItems';
import UOM from './pages/UOM';
import Vouchers from './pages/Vouchers';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<MainLayout />}>
            <Route index element={<Dashboard />} />
            <Route path="companies" element={<Companies />} />
            <Route path="account-groups" element={<AccountGroups />} />
            <Route path="ledgers" element={<Ledgers />} />
            <Route path="stock-groups" element={<StockGroups />} />
            <Route path="stock-items" element={<StockItems />} />
            <Route path="uom" element={<UOM />} />
            <Route path="vouchers" element={<Vouchers />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
