import React, { useEffect, useState } from 'react';
import {
  Typography,
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  CircularProgress,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Grid,
} from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import tallyApi from '../api/tallyApi';

const StockItems = () => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [open, setOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const fetchItems = async () => {
    try {
      setLoading(true);
      const response = await tallyApi.getStockItems();
      if (response.data.success) {
        setItems(response.data.data);
      } else {
        setError(response.data.message);
      }
    } catch (err) {
      setError('Failed to fetch stock items. Is Tally running?');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchItems();
  }, []);

  const formik = useFormik({
    initialValues: {
      name: '',
      alias: '',
      unit: '',
      category: '',
      openingrate: 0,
      openingqnty: 0,
      hsncode: '',
    },
    validationSchema: Yup.object({
      name: Yup.string().required('Required'),
      unit: Yup.string().required('Required'),
      category: Yup.string().required('Required'),
    }),
    onSubmit: async (values) => {
      try {
        setSubmitting(true);
        const response = await tallyApi.saveStockItem(values);
        if (response.data.success) {
          setOpen(false);
          formik.resetForm();
          fetchItems();
        } else {
          alert(response.data.message);
        }
      } catch (err) {
        alert('Failed to save stock item');
        console.error(err);
      } finally {
        setSubmitting(false);
      }
    },
  });

  const handleOpen = () => setOpen(true);
  const handleClose = () => {
    setOpen(false);
    formik.resetForm();
  };

  if (loading && items.length === 0) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Stock Items</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={handleOpen}>
          Add Stock Item
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Alias</TableCell>
              <TableCell>Unit</TableCell>
              <TableCell>Category</TableCell>
              <TableCell>Opening Qty</TableCell>
              <TableCell>Opening Rate</TableCell>
              <TableCell>HSN Code</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((item, index) => (
              <TableRow key={index}>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.alias}</TableCell>
                <TableCell>{item.unit}</TableCell>
                <TableCell>{item.category}</TableCell>
                <TableCell>{item.openingqnty}</TableCell>
                <TableCell>{item.openingrate}</TableCell>
                <TableCell>{item.hsncode}</TableCell>
              </TableRow>
            ))}
            {items.length === 0 && !loading && (
              <TableRow>
                <TableCell colSpan={7} align="center">No items found</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <form onSubmit={formik.handleSubmit}>
          <DialogTitle>Add New Stock Item</DialogTitle>
          <DialogContent>
            <Grid container spacing={2} sx={{ mt: 1 }}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="name"
                  label="Item Name"
                  value={formik.values.name}
                  onChange={formik.handleChange}
                  error={formik.touched.name && Boolean(formik.errors.name)}
                  helperText={formik.touched.name && formik.errors.name}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="alias"
                  label="Alias"
                  value={formik.values.alias}
                  onChange={formik.handleChange}
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  name="unit"
                  label="Unit"
                  value={formik.values.unit}
                  onChange={formik.handleChange}
                  error={formik.touched.unit && Boolean(formik.errors.unit)}
                  helperText={formik.touched.unit && formik.errors.unit}
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  name="category"
                  label="Category/Group"
                  value={formik.values.category}
                  onChange={formik.handleChange}
                  error={formik.touched.category && Boolean(formik.errors.category)}
                  helperText={formik.touched.category && formik.errors.category}
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  type="number"
                  name="openingqnty"
                  label="Opening Quantity"
                  value={formik.values.openingqnty}
                  onChange={formik.handleChange}
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  type="number"
                  name="openingrate"
                  label="Opening Rate"
                  value={formik.values.openingrate}
                  onChange={formik.handleChange}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  name="hsncode"
                  label="HSN Code"
                  value={formik.values.hsncode}
                  onChange={formik.handleChange}
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" disabled={submitting}>
              {submitting ? 'Saving...' : 'Save'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </Box>
  );
};

export default StockItems;
