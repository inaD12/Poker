"use client";

import { useState } from "react";
import { useAuth } from "../../../../users/contexts/AuthContext";
import userService from "../../../../users/services/users.services";
import { LoginUserRequest } from "../../../../users/types/users.types";

const LoginForm = () => {
    const [apiError, setApiError] = useState<string | null>(null);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const { login } = useAuth();

    const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setApiError(null);

        const data: LoginUserRequest = { email, password };

        try {
            await userService.login(data);
            login();
            window.location.href = "/";
        } catch (error: any) {
            if (error.response?.data?.errors) {
                const errorsObj = error.response.data.errors;
                const allErrors = Object.values(errorsObj)
                    .flat()
                    .join('\n');

                setApiError(allErrors);
            } else {
                const errorMessage =
                    error.response?.data.message || "Something went wrong. Please try again.";
                setApiError(errorMessage);
            }
        }

    };

    return (
        <form
            onSubmit={onSubmit}
            className="flex flex-col gap-4 bg-[#437057] w-screen h-screen sm:h-auto sm:min-w-80 sm:max-w-[30%] sm:rounded-xl justify-center text-center text-white p-6 outline-3 outline-white"
        >
            <h1 className="text-4xl mb-6">Login</h1>

            <input
                type="text"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="text-black bg-gray-300 p-2 rounded-md"
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="text-black bg-gray-300 p-2 rounded-md"
            />

            {apiError && <div className="text-red-700 text-sm">{apiError}</div>}

            <button
                type="submit"
                className="mx-auto bg-[#2F5249] text-white p-2 h-[50px] w-[150px] rounded-md hover:bg-[#223b35] transition"
            >
                Submit
            </button>
        </form>
    );
};

export default LoginForm;