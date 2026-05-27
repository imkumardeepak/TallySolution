import React, { useEffect, useState } from 'react';
import {
  Typography,
  Box,
  Paper,
  CircularProgress,
  Alert,
  Card,
  CardContent,
  Divider,
} from '@mui/material';
import { Business as BusinessIcon } from '@mui/icons-material';
import tallyApi from '../api/tallyApi';

const Companies = () => {
  const [company, setCompany] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchCompany = async () => {
    try {
      setLoading(true);
      const response = await tallyApi.getCompany();
      if (response.data.success) {
        setCompany(response.data.data);
      } else {
        setError(response.data.message);
      }
    } catch (err) {
      setError('Failed to fetch company details. Is Tally running?');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCompany();
  }, []);

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Company Information</Typography>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      {company ? (
        <Card sx={{ maxWidth: 500, mt: 3 }}>
          <CardContent>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
              <BusinessIcon color="primary" sx={{ fontSize: 40, mr: 2 }} />
              <Typography variant="h5">{company}</Typography>
            </Box>
            <Divider />
            <Box sx={{ mt: 2 }}>
              <Typography variant="body1" color="textSecondary">
                Currently Active Company in Tally.ERP 9 / TallyPrime
              </Typography>
            </Box>
          </CardContent>
        </Card>
      ) : (
        !error && <Typography>No active company found.</Typography>
      )}
    </Box>
  );
};

export default Companies;
