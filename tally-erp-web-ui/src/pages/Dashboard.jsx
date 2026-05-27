import React from 'react';
import {
  Typography,
  Box,
  Grid,
  Card,
  CardContent,
  CardActionArea,
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {
  Business as BusinessIcon,
  AccountTree as AccountTreeIcon,
  People as PeopleIcon,
  Inventory as InventoryIcon,
  Category as CategoryIcon,
  Straighten as StraightenIcon,
  Receipt as ReceiptIcon,
} from '@mui/icons-material';

const Dashboard = () => {
  const navigate = useNavigate();

  const cards = [
    { title: 'Companies', icon: <BusinessIcon fontSize="large" />, path: '/companies', color: '#1976d2' },
    { title: 'Account Groups', icon: <AccountTreeIcon fontSize="large" />, path: '/account-groups', color: '#2e7d32' },
    { title: 'Ledgers', icon: <PeopleIcon fontSize="large" />, path: '/ledgers', color: '#ed6c02' },
    { title: 'Stock Groups', icon: <CategoryIcon fontSize="large" />, path: '/stock-groups', color: '#9c27b0' },
    { title: 'Stock Items', icon: <InventoryIcon fontSize="large" />, path: '/stock-items', color: '#d32f2f' },
    { title: 'UOM', icon: <StraightenIcon fontSize="large" />, path: '/uom', color: '#0288d1' },
    { title: 'Vouchers', icon: <ReceiptIcon fontSize="large" />, path: '/vouchers', color: '#455a64' },
  ];

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Dashboard</Typography>
      <Typography variant="body1" sx={{ mb: 4 }}>
        Welcome to the Tally ERP Web Portal. Manage your Tally data seamlessly.
      </Typography>

      <Grid container spacing={3}>
        {cards.map((card) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={card.title}>
            <Card sx={{ height: '100%' }}>
              <CardActionArea onClick={() => navigate(card.path)} sx={{ height: '100%' }}>
                <CardContent sx={{ textAlign: 'center', py: 5 }}>
                  <Box sx={{ color: card.color, mb: 2 }}>
                    {card.icon}
                  </Box>
                  <Typography variant="h6">{card.title}</Typography>
                </CardContent>
              </CardActionArea>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default Dashboard;
