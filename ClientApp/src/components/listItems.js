import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import ListSubheader from '@material-ui/core/ListSubheader';
import DashboardIcon from '@material-ui/icons/Dashboard';
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart';
import PeopleIcon from '@material-ui/icons/People';
import SupervisorAccountIcon from '@material-ui/icons/SupervisorAccount';
import AssignmentIcon from '@material-ui/icons/Assignment';
import CategoryIcon from '@material-ui/icons/Category';
import StoreIcon from '@material-ui/icons/Store';
import { Link } from "react-router-dom";

export const mainListItems = (
    <div>
        <ListItem button component={Link} to="/">
            <ListItemIcon>
                <DashboardIcon />
            </ListItemIcon>
            <ListItemText primary="Dashboard" />
        </ListItem>
        <ListItem button>
            <ListItemIcon>
                <ShoppingCartIcon />
            </ListItemIcon>
            <ListItemText primary="Vendas" />
        </ListItem>
        <ListItem button component={Link} to="/client">
            <ListItemIcon>
                <PeopleIcon />
            </ListItemIcon>
            <ListItemText primary="Clientes" />
        </ListItem>
        <ListItem button component={Link} to="/product">
            <ListItemIcon>
                <StoreIcon />
            </ListItemIcon>
            <ListItemText primary="Produtos" />
        </ListItem>

        <ListItem button component={Link} to="/category">
            <ListItemIcon>
                <CategoryIcon />
            </ListItemIcon>
            <ListItemText primary="Categorias" />
        </ListItem>


        <ListItem button component={Link} to="/supplier">
            <ListItemIcon>
                <SupervisorAccountIcon />
            </ListItemIcon>
            <ListItemText primary="Fornecedores" />
        </ListItem>

    </div>
);

export const secondaryListItems = (
    <div>
        <ListSubheader inset>Relatórios salvos</ListSubheader>
        <ListItem button>
            <ListItemIcon>
                <AssignmentIcon />
            </ListItemIcon>
            <ListItemText primary="Mês atual" />
        </ListItem>
        <ListItem button>
            <ListItemIcon>
                <AssignmentIcon />
            </ListItemIcon>
            <ListItemText primary="Último trimestre" />
        </ListItem>
        <ListItem button>
            <ListItemIcon>
                <AssignmentIcon />
            </ListItemIcon>
            <ListItemText primary="Fim de ano" />
        </ListItem>
    </div>
);