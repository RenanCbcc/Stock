import React, { useState } from 'react';
import MaterialTable from 'material-table';
import { makeStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Grid from '@material-ui/core/Grid';
import Alert from '@material-ui/lab/Alert';
import PropTypes from 'prop-types';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';

const baseURL = "api/Client";

function renderItemsTable(handleRowAdd, handleRowUpdate) {

    const columns =
        [
            { title: "id", field: "id", hidden: true },
            {
                title: 'Produto', field: 'description', type: 'string', editable: 'never'

            },
            {
                title: 'Valor', field: 'salePrice', type: 'string', editable: 'never'
            },
            {
                title: 'Quantidade', field: 'quantity', type: 'numeric',
                validate: rowData => rowData.quantity < 0 ? '⚠️ Quantidade não pode ser menor que zero' : ''
            }
        ];

    const localization = {
        body: {
            emptyDataSourceMessage: 'Nenhum registro para exibir',
            addTooltip: 'Adicionar',
            deleteTooltip: 'Apagar',
            editTooltip: 'Editar',
            editRow: {
                deleteText: 'Voulez-vous supprimer cette ligne?',
                cancelTooltip: 'Cancelar',
                saveTooltip: 'Salvar'
            }
        },
        toolbar: {
            searchTooltip: 'Pesquisar',
            searchPlaceholder: 'Pesquisar',
            exportTitle: 'Exportar',
            exportAriaLabel: 'Exportar',
        },
        pagination: {
            labelRowsSelect: 'linhas',
            labelDisplayedRows: '{count} de {from}-{to}',
            firstTooltip: 'Primeira página',
            previousTooltip: 'Página anterior',
            nextTooltip: 'Próxima página',
            lastTooltip: 'Última página'
        },
        header: {
            actions: 'Ações'
        }
    }

    return (
        <MaterialTable
            title="Itens"
            columns={columns}
            localization={localization}
            options={{
                exportButton: true,
                headerStyle: {
                    backgroundColor: '#01579b',
                    color: '#FFF'
                }
            }}
            editable={{
                onRowAdd: newData =>
                    new Promise((resolve) => {
                        handleRowAdd(newData, resolve)
                    }),
                onRowUpdate: (newData, oldData) =>
                    new Promise((resolve) => {
                        handleRowUpdate(newData, oldData, resolve);
                    }),
            }}
        />
    )
};


function renderTabs(classes) {

}

function rederGrid(classes, iserror, errorMessages, handleRowAdd, handleRowUpdate) {
    return (<>
        <div>
            {iserror &&
                <Alert
                    severity="error">
                    {errorMessages.map((msg, i) => {
                        return <div key={i}>{msg}</div>
                    })}
                </Alert>
            }
        </div>
        <div className={classes.root}>
            <Grid container spacing={3}>
                <Grid item xs={8}>
                    {renderItemsTable(handleRowAdd, handleRowUpdate)}
                </Grid>
                <Grid item xs={4}>
                    <p>Abas</p>
                </Grid>
            </Grid>
        </div>
    </>
    )
}


function Sale() {
    const [data, setData] = useState([]);
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);


    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    const handleRowAdd = (newData, resolve) => {
        fetch(baseURL, {
            method: 'Post',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(newData)
        })
            .then(res => isOk(res))
            .then(response => response.json())
            .then(product => {
                let dataToAdd = [...data];
                dataToAdd.push(product);
                setData(dataToAdd);
                resolve()
                setErrorMessages([])
                setIserror(false)
            })
            .catch(error => {
                setErrorMessages([`Não foi possível enviar os dados ao servidor. ${error}`])
                setIserror(true)
                resolve()
            })
    }

    const handleRowUpdate = (newData, oldData, resolve) => {
        newData.status = Number(newData.status);
        fetch(baseURL,
            {
                method: 'Put',
                headers: { 'Content-type': 'application/json' },
                body: JSON.stringify(newData)
            })
            .then(res => isOk(res))
            .then(response => response.json())
            .then(product => {
                const dataUpdate = [...data];
                const index = oldData.tableData.id;
                dataUpdate[index] = product;
                setData([...dataUpdate]);
                resolve()
                setIserror(false)
                setErrorMessages([])
            })
            .catch(error => {
                setErrorMessages(["Não foi possível atualizar o cliente. Erro no servidor."])
                setIserror(true)
                resolve()
            })

    }

    const useStyles = makeStyles((theme) => ({
        root: {
            flexGrow: 1,
        },
        paper: {
            padding: theme.spacing(2),
            textAlign: 'center',
            color: theme.palette.text.secondary,
        },
    }));

    const classes = useStyles();
    return (rederGrid(classes, iserror, errorMessages, handleRowAdd, handleRowUpdate));

};

export default Sale;