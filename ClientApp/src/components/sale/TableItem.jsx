import React, { useState, useEffect } from 'react';
import MaterialTable from 'material-table';

const isOk = (response) => {
    if (response !== null && response.ok) {
        return response;
    } else {
        throw new Error(response.statusText);
    }
}

const columns =
    [
        {
            title: 'Produto', field: 'description', type: 'string', editable: 'never'
        },
        {
            title: 'Valor', field: 'price', type: 'currency', editable: 'never'
        },
        {
            title: 'Quantidade', field: 'quantity', type: 'numeric',
            validate: rowData => rowData.quantity <= 0 ? '⚠️ Quantidade não pode ser menor que zero' : ''
        },
        {
            title: 'SubTotal', field: 'subtotal', type: 'currency', editable: 'never'
        }
    ];

const localization = {
    body: {
        emptyDataSourceMessage: 'Nenhum registro para exibir',
        addTooltip: 'Adicionar',
        deleteTooltip: 'Apagar',
        editTooltip: 'Editar',
        editRow: {
            deleteText: 'Deseja apagar este item?',
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

export function TableItem(props) {

    return (
        <MaterialTable
            title="Itens"
            columns={columns}
            localization={localization}
            data={props.products}
            options={{
                exportButton: true,
                headerStyle: {
                    backgroundColor: '#01579b',
                    color: '#FFF'
                }
            }}
            editable={{
                onRowUpdate: (newData, oldData) =>
                    new Promise((resolve) => {
                        props.onRowUpdate(newData, oldData, resolve);                        
                    }),
                onRowDelete: (oldData) =>
                    new Promise((resolve) => {
                        props.onRowDelete(oldData, resolve);                        
                    }),
            }}
        />
    )
}