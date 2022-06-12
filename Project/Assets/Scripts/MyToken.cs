using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using System;

class MyToken
{
    private static string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"buy\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"budget\",\"type\":\"uint256\"}],\"name\":\"buyWithBudget\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"cancelSale\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"name\":\"setSalePrice\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"tokensOwned\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    public const string chain = "binance";
    public const string network = "testnet";
    public const string contract = "0x31379b10f636124aeaa54789e8cdefcf94123e5e";
    public static string account { get => PlayerPrefs.GetString("Account"); }

    private static BigInteger ParseBigInt(string number)
    {
        try
        {
            return BigInteger.Parse(number);
        }
        catch
        {
            Debug.LogError("Parse number fails: " + number);
            throw;
        }
    }

    #region ERC20
    // The balance of the main currency
    public static async Task<BigInteger> BalanceOf(string _account = null, string _rpc = "")
    {
        string method = "balanceOf";
        string abi = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        if (_account == null) _account = account;
        string[] obj = { _account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return ParseBigInt(response);
    }

    /// The name of the main currency.
    public static async Task<string> Name(string _rpc = "")
    {
        string method = "name";
        //string abi = "[{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"}]";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return response;
    }

    /// The symbol of the main currency.
    public static async Task<string> Symbol(string _rpc = "")
    {
        string method = "symbol";
        //string abi = "[{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"}]";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return response;
    }

    /// The number of decimal places to use for the main currency.
    public static async Task<BigInteger> Decimals(string _rpc = "")
    {
        string method = "decimals";
        //string abi = "[{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"pure\",\"type\":\"function\"}]";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return ParseBigInt(response);
    }

    /// total supply of the main currency.
    public static async Task<BigInteger> TotalSupply(string _rpc = "")
    {
        string method = "totalSupply";
        string abi = "[{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return ParseBigInt(response);
    }

    /// <summary>
    /// TRANSACTION: transfer the main token to the recipient.
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> Transfer(string recipient, BigInteger amount, string _rpc = "")
    {
        string method = "transfer";
        //string abi = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string[] obj = { recipient, amount.ToString() };
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        };
    }
    #endregion

    #region ERC721

    // The address of the owner of the certain token
    public static async Task<string> OwnerOf(BigInteger tokenId, string _rpc = "")
    {
        string method = "ownerOf";
        //string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string[] obj = { tokenId.ToString() };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        return response;
    }

    // Get the list of tokens owned by an account.
    public static async Task<List<BigInteger>> TokensOwned(string account, string _rpc = "")
    {
        string method = "tokensOwned";
        //string abi = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"tokensOwned\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string[] obj = { account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(chain, network, contract, abi, method, args, _rpc);
        try
        {
            string[] responses = JsonConvert.DeserializeObject<string[]>(response);
            List<BigInteger> tokens = new List<BigInteger>();
            for (int i = 0; i < responses.Length; i++)
            {
                tokens.Add(BigInteger.Parse(responses[i]));
            }
            return tokens;
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }

    /// <summary>
    /// TRANSACTION: transfer the token from one user to another
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> TransferToken(string from, string to, BigInteger tokenId)
    {
        string method = "safeTransferFrom";
        //string abi = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string[] obj = { from, to, tokenId.ToString() };
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    /// <summary>
    /// TRANSACTION: mint token for an account. Only admin has this right.
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> MintToken(string to, string tokenId)
    {
        /*
        string keyAccount = "0x8d7090b7E3F8436150DFf41e233B499c0343A45f";
        if (keyAccount != account)
        {
            Debug.LogWarning("You don't have access to this function");
            return;
        }
        //*/
        string method = "mint";
        //string abi = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string[] obj = { to, tokenId.ToString() };
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    #endregion

    #region Buying & selling
    /// <summary>
    /// TRANSACTION: transfer the main token to the recipient.
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> SetSalePrice(BigInteger tokenId, BigInteger price, string _rpc = "")
    {
        string method = "setSalePrice";
        //string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"name\":\"setSalePrice\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string[] obj = { tokenId.ToString(), price.ToString() };
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    /// <summary>
    /// TRANSACTION: cancel the sale of this token
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> CancelSale(BigInteger tokenId, string _rpc = "")
    {
        string method = "setSalePrice";
        string[] obj = { tokenId.ToString(), "0" };
        //string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"cancelSale\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    /// <summary>
    /// TRANSACTION: Buy the token at any cost.
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> Buy(BigInteger tokenId, string _rpc = "")
    {
        string method = "buy";
        string[] obj = { tokenId.ToString() };
        //string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"buy\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    /// <summary>
    /// TRANSACTION: Buy the token if the price is within budget.
    /// </summary>
    /// <returns> the transaction ID </returns>
    public static async Task<string> BuyWithBudget(BigInteger tokenId, BigInteger budget, string _rpc = "")
    {
        string method = "buyWithBudget";
        string[] obj = { tokenId.ToString(), budget.ToString() };
        //string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"budget\",\"type\":\"uint256\"}],\"name\":\"buyWithBudget\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string transactionId = await Web3GL.Send(method, abi, contract, args, "0");
            return transactionId;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        };
    }
    #endregion

    public static async Task<bool> IsTransactionConfirmed(string transactionId, string _rpc = "")
    {
        return await EVM.IsTxConfirmed(chain, network, transactionId, _rpc);
    }
}