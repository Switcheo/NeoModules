using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.TransactionManagers
{
    public class TransactionManager : ITransactionManager
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }

        public IClient Client { get; set; }

        public Task<string> SendRawTransactionAsync(string from, string to, double amount)
        {
            throw new NotImplementedException();
        }
    }
}



//public func sendAssetTransaction(asset: AssetId, amount: Double, toAddress: String, attributes: [TransactionAttritbute]? = nil, completion: @escaping(Bool?, Error?) -> Void)
//{
//    neoClient.getAssets(for: self.address, params: [])
//    {
//        result in
//        switch result {
//            case .failure(let error):
//            completion(nil, error)
//            case .success(let assets):
//            let payload = self.generateSendTransactionPayload(asset: asset, amount: amount, toAddress: toAddress, assets: assets, attributes: attributes)
//            self.neoClient.sendRawTransaction(with: payload) {
//                (result) in
//                switch result {
//                    case .failure(let error):
//                    completion(nil, error)
//                    case .success(let response):
//                    completion(response, nil)
//                }
//            }
//        }
//    }
//}

//func generateSendTransactionPayload(asset: AssetId, amount: Double, toAddress: String, assets: Assets, attributes: [TransactionAttritbute]? = nil) -> Data {
//    var error: NSError?
//        let inputData = getInputsNecessaryToSendAsset(asset: asset, amount: amount, assets: assets)
//    let payloadPrefix: [UInt8] = [0x80, 0x00]
//    let rawTransaction = packRawTransactionBytes(payloadPrefix: payloadPrefix,
//        asset: asset, with: inputData.payload!, runningAmount: inputData.totalAmount!,
//    toSendAmount: amount, toAddress: toAddress, attributes: attributes)
//    let signatureData = NeoutilsSign(rawTransaction, privateKey.fullHexString, &error)
//    let finalPayload = concatenatePayloadData(txData: rawTransaction, signatureData: signatureData!)
//    return finalPayload
//}

/*
* Every asset has a list of transaction ouputs representing the total balance
* For example your total NEO could be represented as a list [tx1, tx2, tx3]
* and each element contains an individual amount. So your total balance would
* be represented as SUM([tx1.amount, tx2.amount, tx3.amount]) In order to make
* a new transaction we will need to find which inputs are necessary in order to
* satisfy the condition that SUM(Inputs) >= amountToSend
*
* We will attempt to get rid of the the smallest inputs first. So we will sort
* the list of unspents in ascending order, and then keep a running sum until we
* meet the condition SUM(Inputs) >= amountToSend. If the SUM(Inputs) == amountToSend 
* then we will have one transaction output since no change needs to be returned
* to the sender. If Sum(Inputs) > amountToSend then we will need two transaction
* outputs, one that sends the amountToSend to the reciever and one that sends
* Sum(Inputs) - amountToSend back to the sender, thereby returning the change.
*
* Input Payload Structure (where each Transaction Input is 34 bytes ). Let n be the
* number of input transactions necessary | Inputs.count | Tx1 | Tx2 |....| Txn |
*
*
*                             * Input Data Detailed View *
* |    1 byte    |         32 bytes         |       2 bytes     | 34 * (n - 2) | 34 bytes |
* | Inputs.count | TransactionId (Reversed) | Transaction Index | ............ |   Txn    |
*
*
*
*                                               * Final Payload *
* | 3 bytes  |    1 + (n * 34) bytes     | 1 byte | 32 bytes |     16 bytes (Int64)     |       32 bytes        |
* | 0x800000 | Input Data Detailed Above |  0x02  |  assetID | toSendAmount * 100000000 | reciever address Hash |
*
*
* |                    16 bytes (Int64)                    |       32 bytes      |  3 bytes |
* | (totalAmount * 100000000) - (toSendAmount * 100000000) | sender address Hash | 0x014140 |
*
*
* |    32 bytes    |      34 bytes        |
* | Signature Data | NeoSigned public key |
*
* NEED TO DOUBLE CHECK THE BYTE COUNT HERE
*/
//        public func getInputsNecessaryToSendAsset(asset: AssetId, amount: Double, assets: Assets) -> (totalAmount: Double?, payload: Data?, error: Error?) {
//        var sortedUnspents = [Unspent]()
//        var neededForTransaction = [Unspent]()
//        if asset == .neoAssetId {
//            if assets.neo.balance<amount {
//                return (nil, nil, NSError())
//            }
//    sortedUnspents = assets.neo.unspent.sorted {$0.value< $1.value }
//        } else {
//            if assets.gas.balance<amount {
//                return (nil, nil, NSError())
//            }
//            sortedUnspents = assets.gas.unspent.sorted { $0.value< $1.value }
//        }
//        var runningAmount = 0.0
//        var index = 0
//        var count: UInt8 = 0
//        //Assume we always have anough balance to do this, prevent the check for bal
//        while runningAmount<amount {
//            neededForTransaction.append(sortedUnspents[index])
//            runningAmount = runningAmount + sortedUnspents[index].value
//            index = index + 1
//            count = count + 1
//        }
//        var inputData = [UInt8]()
//        inputData.append(count)
//        for x in 0..<neededForTransaction.count {
//            let data = neededForTransaction[x].txId.dataWithHexString()
//            let reversedBytes = data.bytes.reversed()
//            inputData = inputData + reversedBytes + toByteArray(UInt16(neededForTransaction[x].index))
//        }

//        return (runningAmount, Data(bytes: inputData), nil)
//}


//        func packRawTransactionBytes(payloadPrefix: [UInt8], asset: AssetId, with inputData: Data, runningAmount: Double,
//                             toSendAmount: Double, toAddress: String, attributes: [TransactionAttritbute]? = nil) -> Data {
//        let inputDataBytes = inputData.bytes
//        let needsTwoOutputTransactions = runningAmount != toSendAmount


//        var numberOfAttributes: UInt8 = 0x00
//        var attributesPayload: [UInt8] = []
//        if attributes != nil {
//            for attribute in attributes! {
//                if attribute.data != nil {
//                    attributesPayload = attributesPayload + attribute.data!
//                    numberOfAttributes = numberOfAttributes + 1
//                }
//}
//        }

//        var payload: [UInt8] = payloadPrefix +  [numberOfAttributes]
//payload = payload + attributesPayload + inputDataBytes
//        if needsTwoOutputTransactions {
//            //Transaction To Reciever
//            payload = payload + [0x02] + asset.rawValue.dataWithHexString().bytes.reversed()
//let amountToSendInMemory = UInt64(toSendAmount* 100000000)
//            payload = payload + toByteArray(amountToSendInMemory)

//            //reciever addressHash
//payload = payload + toAddress.hashFromAddress().dataWithHexString()

//            //Transaction To Sender
//payload = payload + asset.rawValue.dataWithHexString().bytes.reversed()
//            let amountToGetBackInMemory = UInt64(runningAmount* 100000000) - UInt64(toSendAmount* 100000000)
//            payload = payload + toByteArray(amountToGetBackInMemory)
//            payload = payload + hashedSignature.bytes

//        } else {
//            payload = payload + [0x01] + asset.rawValue.dataWithHexString().bytes.reversed()
//let amountToSendInMemory = UInt64(toSendAmount* 100000000)
//            payload = payload + toByteArray(amountToSendInMemory)
//            payload = payload + toAddress.hashFromAddress().dataWithHexString()
//        }
//        return Data(bytes: payload)
//}