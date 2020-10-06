import React, { useState } from 'react';
import MaterialTable from 'material-table';

export function TableItem() {
    const [products, setProduct] = useState([]);

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

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
            },
            {
                title: 'SubTotal', field: 'salePrice', type: 'currency', editable: 'never'
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

        />
    )
}