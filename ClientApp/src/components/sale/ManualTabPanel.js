import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';


const useStyles = makeStyles((theme) => ({
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}));

export function ManualTabPanel() {
    const [category, setCategory] = React.useState('');

    const handleSelectChange = (event) => {
        setCategory(event.target.value);
    };

    const classes = useStyles();

    return (
        <div>
            <FormControl className={classes.formControl}>
                <InputLabel id="demo-simple-select-label">Categoria</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={category}
                    onChange={handleSelectChange}
                >
                    <MenuItem value={10}>Calçados</MenuItem>
                    <MenuItem value={20}>Roupas</MenuItem>
                    <MenuItem value={30}>Cosmesticos</MenuItem>
                </Select>
            </FormControl>
        </div>
    )
}