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
  IconButton,
  Divider,
  Collapse,
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  KeyboardArrowDown as KeyboardArrowDownIcon,
  KeyboardArrowUp as KeyboardArrowUpIcon,
} from '@mui/icons-material';
import { useFormik, FieldArray, FormikProvider } from 'formik';
import * as Yup from 'yup';
import tallyApi from '../api/tallyApi';

const formatDate = (dateStr) => {
  if (!dateStr || dateStr.length !== 8) return dateStr;
  const year = dateStr.substring(0, 4);
  const month = dateStr.substring(4, 6);
  const day = dateStr.substring(6, 8);
  return `${day}/${month}/${year}`;
};

const VoucherRow = ({ item }) => {
  const [open, setOpen] = useState(false);

  return (
    <React.Fragment>
      <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
        <TableCell>
          <IconButton
            aria-label="expand row"
            size="small"
            onClick={() => setOpen(!open)}
          >
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell>{item.remoteID}</TableCell>
        <TableCell>{formatDate(item.date)}</TableCell>
        <TableCell>{item.voucherType}</TableCell>
        <TableCell>{item.partyName}</TableCell>
        <TableCell>{item.accountType}</TableCell>
        <TableCell align="center">{item.items?.length || 0}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={7}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box sx={{ margin: 1 }}>
              <Typography variant="h6" gutterBottom component="div">
                Inventory Details
              </Typography>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Stock Item</TableCell>
                    <TableCell>Quantity</TableCell>
                    <TableCell>Rate</TableCell>
                    <TableCell>Amount</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {item.items?.map((detail, idx) => (
                    <TableRow key={idx}>
                      <TableCell>{detail.stockItemName}</TableCell>
                      <TableCell>{detail.actualQty}</TableCell>
                      <TableCell>{detail.rate}</TableCell>
                      <TableCell>{detail.amount}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </React.Fragment>
  );
};

const Vouchers = () => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [open, setOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const fetchItems = async () => {
    try {
      setLoading(true);
      const response = await tallyApi.getVouchers();
      if (response.data.success) {
        console.log(response.data.data);
        setItems(response.data.data);
      } else {
        setError(response.data.message);
      }
    } catch (err) {
      setError('Failed to fetch vouchers. Is Tally running?');
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
      voucherType: '',
      date: new Date().toISOString().split('T')[0].replace(/-/g, ''),
      partyName: '',
      accountType: '',
      overallamount: '0',
      items: [{ stockItemName: '', rate: '0', amount: '0', actualQty: '0' }],
    },
    validationSchema: Yup.object({
      voucherType: Yup.string().required('Required'),
      partyName: Yup.string().required('Required'),
      accountType: Yup.string().required('Required'),
    }),
    onSubmit: async (values) => {
      try {
        setSubmitting(true);
        const response = await tallyApi.saveVoucher(values);
        if (response.data.success) {
          setOpen(false);
          formik.resetForm();
          fetchItems();
        } else {
          alert(response.data.message);
        }
      } catch (err) {
        alert('Failed to save voucher');
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
        <Typography variant="h4">Vouchers</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={handleOpen}>
          Add Voucher
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell />
              <TableCell>Remote ID</TableCell>
              <TableCell>Date</TableCell>
              <TableCell>Voucher Type</TableCell>
              <TableCell>Party Name</TableCell>
              <TableCell>Account Type</TableCell>
              <TableCell align="center">Items Count</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((item, index) => (
              <VoucherRow key={index} item={item} />
            ))}
            {items.length === 0 && !loading && (
              <TableRow>
                <TableCell colSpan={7} align="center">No vouchers found</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
        <FormikProvider value={formik}>
          <form onSubmit={formik.handleSubmit}>
            <DialogTitle>Add New Voucher</DialogTitle>
            <DialogContent>
              <Grid container spacing={2} sx={{ mt: 1 }}>
                <Grid item xs={6}>
                  <TextField
                    fullWidth
                    name="voucherType"
                    label="Voucher Type (e.g. Sales)"
                    value={formik.values.voucherType}
                    onChange={formik.handleChange}
                    error={formik.touched.voucherType && Boolean(formik.errors.voucherType)}
                    helperText={formik.touched.voucherType && formik.errors.voucherType}
                  />
                </Grid>
                <Grid item xs={6}>
                  <TextField
                    fullWidth
                    name="date"
                    label="Date (YYYYMMDD)"
                    value={formik.values.date}
                    onChange={formik.handleChange}
                  />
                </Grid>
                <Grid item xs={6}>
                  <TextField
                    fullWidth
                    name="partyName"
                    label="Party Name"
                    value={formik.values.partyName}
                    onChange={formik.handleChange}
                    error={formik.touched.partyName && Boolean(formik.errors.partyName)}
                    helperText={formik.touched.partyName && formik.errors.partyName}
                  />
                </Grid>
                <Grid item xs={6}>
                  <TextField
                    fullWidth
                    name="accountType"
                    label="Sales/Purchase Ledger"
                    value={formik.values.accountType}
                    onChange={formik.handleChange}
                    error={formik.touched.accountType && Boolean(formik.errors.accountType)}
                    helperText={formik.touched.accountType && formik.errors.accountType}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    name="overallamount"
                    label="Overall Amount"
                    value={formik.values.overallamount}
                    onChange={formik.handleChange}
                  />
                </Grid>

                <Grid item xs={12}>
                  <Divider sx={{ my: 2 }} />
                  <Typography variant="h6" gutterBottom>Inventory Items</Typography>
                  <FieldArray
                    name="items"
                    render={arrayHelpers => (
                      <Box>
                        {formik.values.items.map((item, index) => (
                          <Grid container spacing={1} key={index} sx={{ mb: 2, alignItems: 'center' }}>
                            <Grid item xs={4}>
                              <TextField
                                fullWidth
                                label="Item Name"
                                name={`items.${index}.stockItemName`}
                                value={item.stockItemName}
                                onChange={formik.handleChange}
                              />
                            </Grid>
                            <Grid item xs={2}>
                              <TextField
                                fullWidth
                                label="Qty"
                                name={`items.${index}.actualQty`}
                                value={item.actualQty}
                                onChange={formik.handleChange}
                              />
                            </Grid>
                            <Grid item xs={2}>
                              <TextField
                                fullWidth
                                label="Rate"
                                name={`items.${index}.rate`}
                                value={item.rate}
                                onChange={formik.handleChange}
                              />
                            </Grid>
                            <Grid item xs={3}>
                              <TextField
                                fullWidth
                                label="Amount"
                                name={`items.${index}.amount`}
                                value={item.amount}
                                onChange={formik.handleChange}
                              />
                            </Grid>
                            <Grid item xs={1}>
                              <IconButton onClick={() => arrayHelpers.remove(index)}>
                                <DeleteIcon />
                              </IconButton>
                            </Grid>
                          </Grid>
                        ))}
                        <Button
                          startIcon={<AddIcon />}
                          onClick={() => arrayHelpers.push({ stockItemName: '', rate: '0', amount: '0', actualQty: '0' })}
                        >
                          Add Item
                        </Button>
                      </Box>
                    )}
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
        </FormikProvider>
      </Dialog>
    </Box>
  );
};

export default Vouchers;
