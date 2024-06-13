import { useRoutes } from "react-router-dom"
import HomePage from "../pages/HomePage"
import LoginPage from "../pages/LoginPage"
import ManageUserPage from "../pages/ManageUserPage"
import CreateUser from "../pages/users/CreateUser"
import ManageAssetPage from "../pages/ManageAssetPage"
import ManageAssignmentPage from "../pages/ManageAssignmentPage"
import RequestForReturningPage from "../pages/RequestForReturningPage"
import ReportPage from "../pages/ReportPage"

export const AppRouter = () => {
    const element = useRoutes([
        {
            path: '/', element:
                <HomePage />
        },
        {
            path: '/login', element:
                <LoginPage />
        },
        {
            path: '/manage-user', element:
                <ManageUserPage />
        },
        {
            path: '/create-user', element:
                <CreateUser />
        },
        {
            path: '/manage-asset', element:
                <ManageAssetPage />
        },
        {
            path: '/manage-assignment', element:
                <ManageAssignmentPage />
        },
        {
            path: '/request-for-returning', element:
                <RequestForReturningPage />
        },
        {
            path: '/report', element:
                <ReportPage />
        },
    ])
    return element;
}