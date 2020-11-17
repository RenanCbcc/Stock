import React from 'react';
import MaterialTable from 'material-table';

export default function OnProducts() {

    const columns =
        [
            { title: "id", field: "id", hidden: true },
            {
                title: 'Descrição', field: 'description', type: 'string'
                
            },
            {
                title: 'Código', field: 'code', type: 'string', editable: 'never'
                
            },
            {
                title: 'Compra', field: 'purchasePrice', type: 'currency'
            },
            {
                title: 'Venda', field: 'salePrice', type: 'currency'
            },
            {
                title: 'Lucro', field: 'profit', type: 'currency'
            },
            {
                title: 'Quantidade', field: 'quantity', type: 'numeric'
            },
            {
                title: 'Desconto', field: 'discount', type: 'numeric'
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

    const operations = (query, data) => {
        //Searching
        data = data.filter(p =>
            p.description.toLowerCase().includes(query.search.toLowerCase()) ||
            p.quantity.includes(query.search) ||
            p.purchasePrice.includes(query.search) ||
            p.salePrice.includes(query.search) ||
            p.discount.includes(query.search) ||
            p.profit.includes(query.search)

        );
        //Sorting 
        if (query.orderBy != null) {
            let field = query.orderBy.field;
            data.sort(function (a, b) {
                if (a[field] > b[field]) {
                    return 1;
                }
                if (a[field] < b[field]) {
                    return -1;
                }
                // a must be equal to b
                return 0;
            });
        }
        return data;
    };

    return (


        <MaterialTable
            title="Sugestão de compra"
            columns={columns}
            localization={localization}
            options={{
                exportButton: true,
                headerStyle: {
                    backgroundColor: '#01579b',
                    color: '#FFF'
                }
            }}
            data={query =>
                new Promise((resolve, reject) => {
                    let url = 'api/Product?'
                    url += 'per_page=' + query.pageSize
                    url += '&page=' + (query.page + 1)
                    fetch(url)
                        .then(response => response.json())
                        .then(result => result.data.map((p) => { p.profit = p.salePrice - p.purchasePrice; return p; }))
                        .then(data => {
                            resolve({
                                data: operations(query, data),
                                page: data.page - 1,
                                totalCount: data.total
                            })
                        }).catch(err => console.log(err))
                })
            }
        />

    )

};

